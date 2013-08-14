using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Base.Log;
using Common.Data;
using Common.Tools;
using Interface;
using TBox.Code.AutoUpdate;
using TBox.Code.FastStart;
using TBox.Code.Managers;
using TBox.Code.Menu;
using TBox.Code.Objects;
using TBox.Forms;
using WPFControls.Code.OS;
using WPFWinForms;

namespace TBox.Code
{
	class UiConfigurator : IDisposable
	{
		private static readonly ILog InfoLog = LogManager.GetInfoLogger<UiConfigurator>();
		private readonly ListBox pluginsList;
		private readonly PluginsSettings pluginsSettings;
		private readonly IMenuItemsProvider menuItemsProvider;
		private readonly ControlsMan controlsMan;
		private readonly MenuMan menuMan;
		private NotifyIconMan notifyIconMan;
		private bool menuRebuildHolded = true;
		private Config cfg;
		public FastStartShower FastStartShower { get; set; }


		public UiConfigurator(
			ListBox pluginsList, 
			ContentControl pluginsBack,
			PluginsSettings pluginsSettings,
			IMenuItemsProvider menuItemsProvider,
			MenuCallsVisitor menuCallsVisitor,
			FastStartDialog fastStartDialog,
			IEnumerable<UMenuItem> menuItems,
			IEnumerable<Pair<PluginName, Control>> existWindows,
			Action<PluginName> onPluginSettingsChanged
			)
		{
			this.pluginsList = pluginsList;
			this.pluginsSettings = pluginsSettings;
			this.menuItemsProvider = menuItemsProvider;
			FastStartShower = new FastStartShower(() => ((MainWindow)Application.Current.MainWindow).ShowAndActivate(), fastStartDialog.ShowAndActivate);

			controlsMan = new ControlsMan(pluginsList, pluginsBack, onPluginSettingsChanged);
			if (existWindows != null)
			{
				foreach (var w in existWindows)
				{
					var ctrl = w;
					controlsMan.Add(w.Key, ()=>ctrl.Value);
				}
			}
			menuMan = new MenuMan(menuItems.ToArray(), menuCallsVisitor);
			pluginsSettings.EnableHotKeys += (o,e)=>Configure();
		}

		public void Init(Window owner, Config config, IAutoUpdater appUpdater, IEnumerable<EnginePluginInfo> toAdd)
		{
			Mt.Do(Sync, () =>
			{
				cfg = config;
				pluginsSettings.Init(appUpdater);
				pluginsSettings.Collection.Clear();
				foreach (var info in toAdd)
				{
					pluginsSettings.Collection.Add(info);
				}
				pluginsSettings.Load(config);
				notifyIconMan = new NotifyIconMan(owner, Properties.Resources.Icon, o => FastStartShower.Show());
				notifyIconMan.NotifyIcon.MouseClick += NotifyIconOnMouseClick;
			});
		}

		private void NotifyIconOnMouseClick(MouseButton mouseButton)
		{
			if (mouseButton == MouseButton.Left && cfg.ShowSettingsByTraySingleClick)
			{
				FastStartShower.Show();
			}
		}

		public void Configure()
		{
			if (notifyIconMan == null || menuRebuildHolded) return;
			var time = Environment.TickCount;
			Mt.Do(Sync, () =>
				{
				menuItemsProvider.Refresh(menuMan.MenuItems.ToArray());
				notifyIconMan.SetMenuItems(menuMan.MenuItems, cfg.UseMenuWithIcons);
				notifyIconMan.NotifyIcon.HoverText = "TBox. Load " + menuMan.Count + " plugin(s).";
				pluginsList.Items.Refresh();
				controlsMan.UpdateSelection();
			});
			InfoLog.Write("Refresh menu and plugins settings time: {0}", Environment.TickCount - time);
		}

		public void ConfigurePlugin(IPlugin plugin, string name)
		{
			var cplg = plugin as IConfigurablePlugin;
			if (notifyIconMan == null || menuRebuildHolded) return;
			Mt.Do(Sync, () => {
				UMenuItem[] pluginMenu = null;
				if (cplg != null)
				{
					cplg.OnRebuildMenu();
					pluginMenu = cplg.Menu.ToArray();
					menuMan.SetMenuItems(name, pluginMenu);
				}
				else
				{
					pluginMenu = menuMan.GetMenuItems(name).ToArray();
				}
				menuItemsProvider.Refresh(name, pluginMenu);
				notifyIconMan.UpdateSubMenuItems(name, pluginMenu);
				notifyIconMan.NotifyIcon.HoverText = "ToolBox. Load " + menuMan.Count + " plugin(s).";
			});
		}

		public void DoHoldOperation(Action a)
		{
			try
			{
				menuRebuildHolded = true;
				a();
			}
			finally
			{
				menuRebuildHolded = false;
				Configure();
			}
		}

		public void SetPluginCheck(string key, bool value)
		{
			pluginsSettings.Collection.SetCheck(key, value);
		}

		public void Load(Config config)
		{
			cfg = config;
			Mt.Do(Sync, ()=>pluginsSettings.Load(config));
		}

		public void Save(Config config)
		{
			Configure();
			Mt.Do(Sync, controlsMan.Refresh);
			pluginsSettings.Save(config);
		}

		public PluginUi InitPlugin(IPlugin plugin, string key)
		{
			Func<Control> settingsGetter = null;
			var cfgPlg = plugin as IConfigurablePlugin;
			if (cfgPlg != null)
			{
				settingsGetter = cfgPlg.SettingsGetter;
			}
			var info = pluginsSettings.Collection.First(x => key.EqualsIgnoreCase(x.Key));
			var menu = plugin.Menu;
			info.Plugin = plugin;
			return new PluginUi
				       {
						   Key = key,
					       Name = new PluginName(info.Name, info.Description), 
						   Icon = plugin.Icon, 
						   Menu = menu, 
						   Settings = settingsGetter
				       };
		}

		public void Remove(string key)
		{
			var info = pluginsSettings.Collection.First(x => key.EqualsIgnoreCase(x.Key));
			menuMan.Remove(info.Name);
			controlsMan.Remove(info.Name);
		}

		public IEnumerable<string> GetPluginsStates(IList<string> toAdd, IList<string> toRemove, IList<string> disabledBefore, PluginsMan plugMan )
		{
			foreach (var t in pluginsSettings.Collection)
			{
				var name = t.Key;
				var enabled = t.IsChecked;
				if (enabled == disabledBefore.Contains(name))
				{
					(enabled ? toAdd : toRemove).Add(name);
				}
				else if (enabled)
				{
					var plugin = plugMan.Get(name);
					menuMan.Change(t.Name, plugin.Icon, plugin.Menu);
				}
				if (!enabled)
				{
					yield return name;
				}
			}
		}

		public void Dispose()
		{
			if (notifyIconMan!=null) notifyIconMan.Dispose();
			pluginsSettings.Dispose();
		}

		public Control Sync { get { return pluginsSettings; } }

		public void InitUi(List<PluginUi> ret)
		{
			foreach (var p in ret.OrderBy(x=>x.Name.ToString()))
			{
				if (p.Menu != null) menuMan.Add(p.Name.Name, p.Icon, p.Menu);
				if (p.Settings != null) controlsMan.Add(p.Name, p.Settings);
			}
		}
	}
}
