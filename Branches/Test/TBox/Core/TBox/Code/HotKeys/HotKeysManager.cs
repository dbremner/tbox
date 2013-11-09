﻿using System;
using System.Collections.Generic;
using System.Linq;
using Localization.TBox;
using TBox.Code.HotKeys.Settings;
using TBox.Code.Menu;
using WPFControls.Components.Units;
using WPFControls.Tools;
using WPFWinForms;
using WPFWinForms.GlobalHotKeys;

namespace TBox.Code.HotKeys
{
	sealed class HotKeysManager : IDisposable
	{
		private GlobalHotkeys globalHotkeys = null;
		private readonly CheckableListBoxUnit view;
		private readonly IMenuItemsProvider menuItemsProvider;
		private HotKeyTasks config;
		private HotKeyTasks originalConfig;
		public HotKeysManager(CheckableListBoxUnit view, IMenuItemsProvider menuItemsProvider )
		{
			this.view = view;
			this.menuItemsProvider = menuItemsProvider;
			menuItemsProvider.OnRefresh += (o,e)=>Refresh();
			menuItemsProvider.OnRefreshItem += RefreshItem;
		}

		public void Refresh()
		{
			Dispose();
			ClearHotKeys(menuItemsProvider.GetItems());
			view.ConfigureInputMenuItem(TBoxLang.HotKeys, 
				originalConfig.Tasks, 
				menuItemsProvider.GetDialogItems(), 
				validator: x => true);
			if (!config.IsEnabled)return;
			globalHotkeys = new GlobalHotkeys();
			foreach (var item in config.Tasks.CheckedItems.OrderBy(x=>x.Key))
			{
				UpdateMenu(item);
			}
		}

		internal void RefreshItem(string name)
		{
			ClearHotKeys(menuItemsProvider.GetItems());
			if (!config.IsEnabled)
			{
				return;
			}
			foreach (var item in config.Tasks.CheckedItems.OrderBy(x => x.Key).Where(x=>x.Key.StartsWith(name, StringComparison.OrdinalIgnoreCase)))
			{
				UpdateMenu(item);
			}
		}

		private static void ClearHotKeys(IEnumerable<UMenuItem> menuItems)
		{
			foreach (var item in menuItems)
			{
				item.HotKey = null;
				ClearHotKeys(item.Items);
			}
		}

		private void UpdateMenu(HotKeyTask item)
		{
			var menu = menuItemsProvider.Get(item.Key);
			if (menu == null || menu.Items.Count > 0 || item.HotKey == null) return;
			menu.HotKey = item.HotKey.ToString();
			globalHotkeys.RegisterHotKey(item.HotKey, ()=>menu.OnClick(null));
		}

		public void OnConfigUpdated(HotKeyTasks cfg)
		{
			originalConfig = cfg;
			config = cfg.Clone();
		}

		public void Dispose()
		{
			if (globalHotkeys == null) return;
			globalHotkeys.Dispose();
			globalHotkeys = null;
		}
	}
}
