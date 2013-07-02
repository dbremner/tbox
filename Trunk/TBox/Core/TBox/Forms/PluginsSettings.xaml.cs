﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Common.OS;
using Common.Tools;
using Common.UI.ModelsContainers;
using TBox.Code;
using TBox.Code.AutoUpdate;
using TBox.Code.FastStart;
using TBox.Code.FastStart.Settings;
using TBox.Code.HotKeys;
using TBox.Code.Menu;
using TBox.Code.Objects;
using TBox.Code.Shelduler;
using TBox.Code.Themes;
using WPFControls.Components;

namespace TBox.Forms
{
	/// <summary>
	/// Interaction logic for PluginsSettings.xaml
	/// </summary>
	sealed partial class PluginsSettings : IDisposable
	{
		private static readonly  string FilePath = Process.GetCurrentProcess().MainModule.FileName;
		public CheckableDataCollection<EnginePluginInfo> Collection { get; private set; }
		private AppUpdater appUpdater;
		private readonly ThemesManager themesManager = new ThemesManager();
		public event Action EnableHotKeys;
		private readonly HotKeysManager hotKeysManager;
		private readonly SchedulerManager schedulerManager;
		private readonly UserActionsManager userActionsManager;

		public PluginsSettings(IMenuItemsProvider menuItemsProvider)
		{
			InitializeComponent();
			Version.Content = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;
			Panel.ItemsSource = Collection = new CheckableDataCollection<EnginePluginInfo>();
			PanelButtons.View = Panel;
			hotKeysManager = new HotKeysManager(HotKeysView, menuItemsProvider);
			schedulerManager = new SchedulerManager(ScheldulerView, menuItemsProvider);
			userActionsManager = new UserActionsManager(UserActionsView, menuItemsProvider);
			Themes.ItemsSource = themesManager.AvailableThemes;
		}

		public void Init(AppUpdater updater)
		{
			appUpdater = updater;
		}

		public void Load(Config cfg)
		{
			DataContext = cfg;
			OnConfigUpdated(cfg);
		}

		public void Save(Config cfg)
		{
			OnConfigUpdated(cfg);
		}

		private void OnConfigUpdated(Config cfg)
		{
			hotKeysManager.OnConfigUpdated(cfg.HotKeys);
			schedulerManager.OnConfigUpdated(cfg.SchedulerTasks);
			userActionsManager.OnConfigUpdated(cfg.FastStartConfig);
		}

		private void GcCollectClick(object sender, RoutedEventArgs e)
		{
			GC.Collect();
			GC.WaitForFullGCComplete();
		}

		private void ShortcutToDesktopClick(object sender, RoutedEventArgs e)
		{
			Shortcuts.CreateOnDesktop(FilePath);
		}

		private void ShortcutToAutorunClick(object sender, RoutedEventArgs e)
		{
			Shortcuts.CreateOnAutorun(FilePath);
		}

		private void OnCheckChangedEvent(object sender, RoutedEventArgs e)
		{
			Panel.OnCheckChangedEvent(sender, e);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!appUpdater.TryUpdate(true))
				MessageBox.Show("No updates found", "TBox", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}

		private void HotKeysEnabledChecked(object sender, RoutedEventArgs e)
		{
			if (EnableHotKeys != null ) EnableHotKeys();
		}

		private void SheldulerEnabledChecked(object sender, RoutedEventArgs e)
		{
			if (EnableHotKeys != null) EnableHotKeys();
		}

		private void ButtonClearHotkeyClick(object sender, RoutedEventArgs e)
		{
			((HotKeyBox) ((DockPanel) ((Button) sender).Parent).Children[2]).HotKey = null;
		}

		private void ThemeChanged(object sender, RoutedEventArgs e)
		{
			themesManager.Load(Themes.Value);
		}
		
		private void InformationSelected(object sender, RoutedEventArgs e)
		{
			const string changeLogFilePath = "changelog.txt";
			if (File.Exists(changeLogFilePath))
			{
				tbChangeLog.Text = File.ReadAllText(changeLogFilePath);
			}
		}

		public void Dispose()
		{
			hotKeysManager.Dispose();
			schedulerManager.Dispose();
		}

		private void ScheldulerCheckChanged(object sender, RoutedEventArgs e)
		{
			ScheldulerView.OnCheckChangedEvent(sender, e);
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			using (var p = Process.Start("https://tbox.codeplex.com/")) { }
		}

		private void UserActionsCheckChanged(object sender, RoutedEventArgs e)
		{
			UserActionsView.OnCheckChangedEvent(sender,e);
		}

		private void BtnEditUserActionClick(object sender, RoutedEventArgs e)
		{
			var selectedKey = ((TextBlock)((DockPanel)((Button)sender).Parent).Children[2]).Text;
			var cfg = ((Config) DataContext).FastStartConfig.MenuItemsSequence.GetExistByKeyIgnoreCase(selectedKey);
			userActionsManager.ShowDialog(cfg);
		}

		private void ButtonClearHistoryClick(object sender, RoutedEventArgs e)
		{
			((Config) DataContext).FastStartConfig.MenuItems.Clear();
		}
	}
}
