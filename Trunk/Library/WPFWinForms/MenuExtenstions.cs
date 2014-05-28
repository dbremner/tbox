using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mnk.Library.Common.Log;

namespace Mnk.Library.WpfWinForms
{
	public static class MenuExtenstions
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MenuExtenstions));
		public static ToolStripItem[] ToStripMenuItems(this IList<UMenuItem> menuItems)
		{
			var items = new ToolStripItem[menuItems.Count];
			for (var i = 0; i < menuItems.Count;++i )
			{
				var item = menuItems[i];
				if (item is USeparator)
				{
					items[i] = new ToolStripSeparator();
				}
				else
				{
					var menu = new ToolStripMenuItem(item.Header);
					if (item.Items.Any())
					{
						menu.DropDownItems.AddRange(item.Items.ToStripMenuItems());
					}
					else if (item.OnClick != null)
					{
						menu.Click += (o,e) => SafeClick(item, o);
					}
					if (item.Icon != null)
					{
						menu.Image = item.Icon.ToBitmap();
						menu.ImageScaling = ToolStripItemImageScaling.SizeToFit;
					}
					if (item.Hotkey != null)
					{
						menu.ShowShortcutKeys = true;
						menu.ShortcutKeyDisplayString = item.Hotkey;
					}
					menu.Enabled = item.IsEnabled;
					items[i] = menu;
				}
			}
			return items;
		}

		private static void SafeClick(UMenuItem item, object o)
		{
			try
			{
				item.OnClick(o);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't execute menu item: " + item.Header);
			}
		}

		public static ContextMenuStrip ToWinFormsStripMenu(this IList<UMenuItem> menuItems)
		{
			var menu = new ContextMenuStrip();
			menu.Items.AddRange(menuItems.ToArray().ToStripMenuItems());
			return menu;
		}

		public static MenuItem[] ToMenuItems(this IList<UMenuItem> menuItems)
		{
			var items = new MenuItem[menuItems.Count];
			for (var i = 0; i < menuItems.Count; ++i)
			{
				var item = menuItems[i];
				if (item is USeparator)
				{
					items[i] = new MenuItem{Text = "-"};
				}
				else
				{
					var menu = new MenuItem(item.Header);
					if (item.Hotkey != null)
					{
						menu.Text += '\t' + item.Hotkey;
					}
					if (item.Items.Any())
					{
						menu.MenuItems.AddRange(item.Items.ToMenuItems());
					}
					else if (item.OnClick != null)
					{
						menu.Click += (o, e) => SafeClick(item, o);
					}
					menu.Enabled = item.IsEnabled;
					items[i] = menu;
				}
			}
			return items;
		}

		public static ContextMenu ToWinFormsMenu(this IList<UMenuItem> menuItems)
		{
			var menu = new ContextMenu();
			menu.MenuItems.AddRange(menuItems.ToArray().ToMenuItems());
			return menu;
		}

	}
}
