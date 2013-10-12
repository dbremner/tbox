using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Common.Base.Log;
using Common.Plugins;
using Interface;
using Interface.Atrributes;
using Localization.TBox;
using TBox.Code.AutoUpdate;
using TBox.Code.Managers;
using TBox.Code.Objects;
using WPFControls.Code.OS;
using WPFControls.Localization;
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
		private readonly string pluginsReadOnlyDataFolder = Path.Combine(Environment.CurrentDirectory, "Data");
		private readonly string pluginsStoreDataFolder = Path.Combine(Folders.UserFolder, "Data");
		private readonly string toolsDataFolder = Path.Combine(Environment.CurrentDirectory, "Tools");
		private readonly IAutoUpdater appUpdater;
		private readonly IconsCache iconsCache = new IconsCache();
		private readonly IconsExtractor iconsExtractor = new IconsExtractor();
		private readonly WarmingUpManager warmingUpManager = new WarmingUpManager();
		private readonly PluginsContextShared pluginsContextShared;
		public ConfigManager ConfigManager { get; private set; }

		public Engine(Window owner, IUpdater updater, UiConfigurator uiConfigurator, ConfigManager configManager)
		{
			var time = Environment.TickCount;
			ConfigManager = configManager;
			this.uiConfigurator = uiConfigurator;
			InfoLog.Write("Load first config time: {0}", Environment.TickCount - time);
			updater.Update(TBoxLang.CheckForUpdates, 0.06f);
			appUpdater = new ApplicationUpdater(owner, ConfigManager.Config, new CodePlexUpdater() /*new DirectoryApplicationUpdater(config.Update.Directory)*/);
			ThreadPool.QueueUserWorkItem(o=>appUpdater.TryUpdate());
			updater.Update(TBoxLang.Prepare, 0.07f);
			plugMan = new PluginsMan(
				new Factory<IPlugin>(
					Path.Combine(Environment.CurrentDirectory, "Plugins"),
					Path.Combine(Environment.CurrentDirectory, "Libraries")
					),
				Path.Combine(Folders.UserFolder, "Config"),
				a=>Mt.Do(owner, a)
				);
			var toAdd = new List<string>();
			pluginsContextShared = new PluginsContextShared(iconsCache, iconsExtractor, uiConfigurator.Sync, warmingUpManager);
			uiConfigurator.Init(owner, ConfigManager.Config, appUpdater, SetupPlugins(toAdd));
			AddPlugins(toAdd, updater);
			uiConfigurator.ShowTrayIcon(owner);
		}

		private IEnumerable<EnginePluginInfo> SetupPlugins(ICollection<string> toAdd )
		{
			foreach (var name in plugMan.Factory.Names.OrderBy(x => x))
			{
				var t = plugMan.Factory.Get(name);
				var a = GetAttribute<PluginInfoAttribute>(t);
				var icon = a.IsIconSystem?pluginsContextShared.GetSystemIcon(a.SystemIconId):a.Icon;
				var enabled = !ConfigManager.Config.DisabledItems.Contains(name);
				yield return new EnginePluginInfo(name, a.Name, a.Description, icon, enabled, a.PluginGroup);
				if (enabled)
				{
					toAdd.Add(name);
				}
			}
		}

		private static T GetAttribute<T>(Type t) where T : Attribute
		{
			var attributes = t.GetCustomAttributes(typeof(T), false);
			if (!attributes.Any()) return null;
			var a = attributes[0] as T;
			if(a == null) throw new ArgumentException("Plugin should have valid attribute");
			return a;
		}

		private void AddPlugins(IEnumerable<string> names, IUpdater updater)
		{
			updater.Update(TBoxLang.CreatePlugins, 0.33f);
			CreatePlugins(names.ToArray(), updater);
			warmingUpManager.CreateAll();
		}

		private void CreatePlugins(string[] names, IUpdater updater)
		{
			var time = Environment.TickCount;
			var ret = new List<EnginePluginInfo>(names.Length);
			var culture = Translator.Culture;
			Parallel.ForEach(names, name => CreatePlugin(names, updater, name, ret, culture));
			uiConfigurator.InitUi(ret);
			InfoLog.Write("Create all plugin time: {0}", Environment.TickCount - time);
		}

		private void CreatePlugin(IEnumerable<string> names, IUpdater updater, string name, List<EnginePluginInfo> ret, CultureInfo culture)
		{
			Translator.Culture = culture;
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
			updater.Update(TBoxLang.Create + ": " + (ui!=null? ui.Name:name), 0.3f + (0.7f*ret.Count)/names.Count());
		}

		private void HideCheck(string key)
		{
			uiConfigurator.SetPluginCheck(key, false);
			ConfigManager.Config.DisabledItems.Add(key);
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
			ConfigManager.Load();
			foreach (var name in plugMan.Factory.Names)
			{
				uiConfigurator.SetPluginCheck(name, !ConfigManager.Config.DisabledItems.Contains(name));
			}
			plugMan.Load(updater);
			uiConfigurator.Load(ConfigManager.Config);
		}

		public void Save(IUpdater updater, bool autoSaveOnExit)
		{
			uiConfigurator.DoHoldOperation(() => DoSave(updater, autoSaveOnExit));
		}

		private void DoSave(IUpdater updater, bool autoSaveOnExit)
		{
			updater.Update(TBoxLang.UpdateDisabledItems, 0);
			var disabledBefore = new List<string>(ConfigManager.Config.DisabledItems);
			ConfigManager.Config.DisabledItems.Clear();
			var toAdd = new List<string>();
			var toRemove = new List<string>();
			plugMan.Save(updater, autoSaveOnExit);
			foreach (var item in uiConfigurator.GetPluginsStates(toAdd, toRemove, disabledBefore, plugMan))
			{
				ConfigManager.Config.DisabledItems.Add(item);
			}
			AddPlugins(toAdd, updater);
			RemovePlugins(toRemove);
			updater.Update(TBoxLang.Save, 1.0f);
			uiConfigurator.Save(ConfigManager.Config);
			ConfigManager.Save();
		}

		public EnginePluginInfo InitPlugin(string key, IPlugin plugin)
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
				ConfigManager.Save();
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
			if (!silent) MessageBox.Show(TBoxLang.MessageNoUpdatesFound, TBoxLang.AppName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
	}
}
