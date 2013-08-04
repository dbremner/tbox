using System;
using System.Collections.Generic;
using System.Linq;
using TBox.Code.FastStart.Settings;
using TBox.Code.Menu;
using WPFWinForms;

namespace TBox.Code.FastStart
{
	class RecentItemsCollector : IMenuRunHandler
	{
		private readonly IList<MenuItemStatistic> menuItems = new List<MenuItemStatistic>();
		private readonly IMenuItemsProvider menuItemsProvider;

		public RecentItemsCollector(IList<MenuItemStatistic> menuItems, IMenuItemsProvider menuItemsProvider)
		{
			this.menuItems = menuItems;
			this.menuItemsProvider = menuItemsProvider;
			menuItemsProvider.OnRefresh += (o,e)=>RefreshMenuItems();
			menuItemsProvider.OnRefreshItem += name => RefreshMenuItems();
			RefreshMenuItems();
		}

		public void Handle(UMenuItem item, string[] path)
		{
			//skip system menu items
			if(path.Length < 2 )return;
			lock (menuItems)
			{
				var realPath = string.Join(Environment.NewLine, path);
				foreach (var menuItem in menuItems.Where(x => string.Equals(x.Path, realPath)))
				{
					menuItem.Count++;
					return;
				}
				menuItems.Add(
					new MenuItemStatistic
						{
							IsValid = true,
							Icon = item.Icon ?? menuItemsProvider.GetRoot(path[0]).Icon,
							OnClick = item.OnClick,
							Path = realPath, 
							Count = 1
						}
					);
			}
		}

		public IEnumerable<MenuItemStatistic> GetStatistic(int count)
		{
			lock (menuItems)
			{
				return menuItems
					.Where(x=>x.IsValid)
					.OrderBy(x => -x.Count)
					.Take(count)
					.Select(x=>x.Clone())
					.ToArray();
			}
		}

		private void RefreshMenuItems()
		{
			foreach (var item in menuItems)
			{
				var menu = menuItemsProvider.Get(item.Path);
				if (menu == null)
				{
					item.IsValid = false;
					continue;
				}
				var root = item.Path.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).First();
				item.IsValid = true;
				item.Icon = menu.Icon ?? menuItemsProvider.GetRoot(root).Icon;
				item.OnClick = menu.OnClick;
			}
		}

	}
}
