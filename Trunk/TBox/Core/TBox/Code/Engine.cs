using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Common.Base.Log;
using Common.Plugins;
using Common.SaveLoad;
using Common.Tools;
using Interface;
using Interface.Atrributes;
using TBox.Code.AutoUpdate;
using TBox.Code.Managers;
using TBox.Code.Objects;
using WPFControls.Code.OS;
using WPFWinForms.Icons;
using IUpdater = Common.MT.IUpdater;

namespace TBox.Code
{
	class Engine : IDisposable
	{
		private static readonly ILog InfoLog = LogManager.GetInfoLogger<Engine>();
		private static readonly ILog Log = LogManager.GetLogger<Engine>();
		private readonly UiConfigurator uiConfigurator;
		private readonly PluginsMan plugMan;
		public Config Config { get; private set; }
		private readonly ParamSerializer<Config> paramSer;
		private readonly string configFile = Path.Combine(Folders.UserFolder, "Config.config");
		private readonly string pluginsReadOnlyDataFolder = Path.Combine(Environment.CurrentDirectory, "Data");
		private readonly string pluginsStoreDataFolder = Path.Combine(Folders.UserFolder, "Data");
		private readonly string toolsDataFolder = Path.Combine(Environment.CurrentDirectory, "Tools");
		private readonly AppUpdater appUpdater;
		private readonly IconsCache iconsCache = new IconsCache();
		private readonly IconsExtractor iconsExtractor = new IconsExtractor();
		private readonly WarmingUpManager warmingUpManager = new WarmingUpManager();
		private readonly PluginsContextShared pluginsContextShared;

		public bool IsInitialized { get; private set; }

		public Engine(Window owner, IUpdater updater, UiConfigurator uiConfigurator)
		{
			var time = Environment.TickCount;
			this.uiConfigurator = uiConfigurator;
			updater.Update("Load configuration...", 0.01f);
			CopySystemConfigIfNeed();
			paramSer = new ParamSerializer<Config>(configFile);
			Config = paramSer.Load(Config=new Config());
			InfoLog.Write("Load first config time: {0}", Environment.TickCount - time);
			updater.Update("Check for updates...", 0.06f);
			appUpdater = new AppUpdater(owner, Config);
			IsInitialized = !appUpdater.TryUpdate();
			if (!IsInitialized) return;
			updater.Update("Prepare...", 0.07f);
			plugMan = new PluginsMan(
				new Factory<IPlugin>(
					Path.Combine(Environment.CurrentDirectory, "Plugins"),
					Path.Combine(Environment.CurrentDirectory, "Libraries")
					),
				Path.Combine(Folders.UserFolder, "Config"),
				a=>Mt.Do(owner, a)
				);
			var toAdd = new List<string>();
			uiConfigurator.Init(owner, Config, appUpdater, SetupPlugins(toAdd));
			pluginsContextShared = new PluginsContextShared(iconsCache, iconsExtractor, uiConfigurator.Sync, warmingUpManager);
			AddPlugins(toAdd, updater);
			uiConfigurator.DoHoldOperation(()=> { });
		}

		private void CopySystemConfigIfNeed()
		{
			try
			{
                new FileInfo(Path.Combine(Environment.CurrentDirectory, "Config.config"))
                    .MoveIfExist(configFile);
			}
			catch {}

			try
			{
                new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Config"))
                    .MoveIfExist(Path.Combine(Folders.UserFolder, "Config"));
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't migrate config files");
			}

		}

		private IEnumerable<EnginePluginInfo> SetupPlugins(ICollection<string> toAdd )
		{
			foreach (var name in plugMan.Factory.Names.OrderBy(x => x))
			{
				var t = plugMan.Factory.Get(name);
				var pluginName = GetAttributeValue<PluginNameAttribute>(t, name);
				var pluginDescription = GetAttributeValue<PluginDescriptionAttribute>(t, string.Empty);
				var enabled = !Config.DisabledItems.Contains(name);
				yield return new EnginePluginInfo(name, pluginName, pluginDescription, enabled);
				if (enabled)
				{
					toAdd.Add(name);
				}
			}
		}

		private static string GetAttributeValue<T>(Type t, string defValue) where T : ValueAttribute
		{
			var attributes = t.GetCustomAttributes(typeof (T), false);
			if(attributes.Any())
			{
				var a = attributes[0] as T;
				if(a!=null)
				{
					return a.Value;
				}
			}
			return defValue;
		}

		private void AddPlugins(IEnumerable<string> names, IUpdater updater)
		{
			updater.Update("Create plugins..", 0.33f);
			CreatePlugins(names.ToArray(), updater);
			warmingUpManager.CreateAll();
		}

		private void CreatePlugins(string[] names, IUpdater updater)
		{
			var time = Environment.TickCount;
			var ret = new List<PluginUi>(names.Length);
			Parallel.ForEach(names, name => CreatePlugin(names, updater, name, ret));
			uiConfigurator.InitUi(ret);
			InfoLog.Write("Create all plugin time: {0}", Environment.TickCount - time);
		}

		private void CreatePlugin(IEnumerable<string> names, IUpdater updater, string name, List<PluginUi> ret)
		{
			var time = Environment.TickCount;
			var plg = Add(name);
			if (plg == null) return;
			var pmu = new PluginMenuUpdater(x => uiConfigurator.ConfigurePlugin(plg, x));
			plg.Init(
				new PluginContext(
					new DataProvider(toolsDataFolder, Path.Combine(pluginsReadOnlyDataFolder, name), Path.Combine(pluginsStoreDataFolder, name)),
					pmu.Do,
					pluginsContextShared));
			var ui = InitPlugin(name, plg);
			if (ui != null)
			{
				pmu.Init(ui.Name);
				lock (ret)
				{
					ret.Add(ui);
				}
			}
			InfoLog.Write("Create plugin time: {1}, log : '{0}'", name, Environment.TickCount - time);
			updater.Update("Create: " + name, 0.3f + (0.7f*ret.Count)/names.Count());
		}

		private void HideCheck(string key)
		{
			uiConfigurator.SetPluginCheck(key, false);
			Config.DisabledItems.Add(key);
		}

		private void RemovePlugins(IEnumerable<string> names)
		{
			Mt.Do(uiConfigurator.Sync, 
				() =>{
						foreach (var name in names.ToArray())
						{
							Remove(name);
						}
				    });
		}

		public void Load(IUpdater updater)
		{
			uiConfigurator.DoHoldOperation(()=>DoLoad(updater));
		}

		private void DoLoad(IUpdater updater)
		{
			Config = paramSer.Load(Config = new Config());
			foreach (var name in plugMan.Factory.Names)
			{
				uiConfigurator.SetPluginCheck(name, !Config.DisabledItems.Contains(name));
			}
			plugMan.Load(updater);
			uiConfigurator.Load(Config);
		}

		public void Save(IUpdater updater, bool autoSaveOnExit)
		{
			uiConfigurator.DoHoldOperation(() => DoSave(updater, autoSaveOnExit));
		}

		private void DoSave(IUpdater updater, bool autoSaveOnExit)
		{
			updater.Update("Update disabled items...", 0);
			var disabledBefore = new List<string>(Config.DisabledItems);
			Config.DisabledItems.Clear();
			var toAdd = new List<string>();
			var toRemove = new List<string>();
			plugMan.Save(updater, autoSaveOnExit);
			foreach (var item in uiConfigurator.GetPluginsStates(toAdd, toRemove, disabledBefore, plugMan))
			{
				Config.DisabledItems.Add(item);
			}
			AddPlugins(toAdd, updater);
			RemovePlugins(toRemove);
			updater.Update("Save...", 1.0f);
			uiConfigurator.Save(Config);
			paramSer.Save(Config);
		}

		public PluginUi InitPlugin(string key, IPlugin plugin)
		{
			try
			{
				plugMan.LoadItem(key);
				return uiConfigurator.InitPlugin(plugin, key);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error init: '{0}'", key);
				Remove(key);
				HideCheck(key);
			}
			return null;
		}

		private IPlugin Add(string key)
		{
			IPlugin plg = null;
			try
			{
				plg = plugMan.Create(key);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't create plugin: '{0}'", key);
			}
			if (plg == null)
			{
				HideCheck(key);
			}
			return plg;
		}

		private void Remove(string key)
		{
			uiConfigurator.Remove(key);
			plugMan.Remove(key);
		}

		public bool Close(IUpdater updater, bool criticalError)
		{
			if (!criticalError)
			{
				plugMan.Save(updater, true);
				paramSer.Save(Config);
			}
			RemovePlugins(plugMan.Factory.Names);
			return true;
		}

		public void Dispose()
		{
			iconsCache.Dispose();
		}

		public void CheckUpdates(bool silent)
		{
			if (appUpdater.TryUpdate(true)) return;
			if (!silent) MessageBox.Show("No updates found", "TBox", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
	}
}
