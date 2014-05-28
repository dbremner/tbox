using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Mnk.Library.WpfWinForms.Icons
{
	public sealed class TrayIcon : IDisposable
	{
		private readonly NotifyIcon notifyIcon = new NotifyIcon();
		
		public event Action<MouseButton> MouseClick;
		public event EventHandler DoubleClick;

		public TrayIcon()
		{
			notifyIcon.MouseClick += NotifyIconMouseClick;
			notifyIcon.DoubleClick += NotifyIconDoubleClick;
		}

		private static MouseButton GetMouseButton(MouseEventArgs e)
		{
			switch(e.Button)
			{
				case MouseButtons.Left: return MouseButton.Left;
				case MouseButtons.Middle: return MouseButton.Middle;
				case MouseButtons.Right: return MouseButton.Right;
				default: return MouseButton.XButton1;
			}
		}

		private void NotifyIconDoubleClick(object sender, EventArgs e)
		{
			if (DoubleClick != null) DoubleClick(this, e);
		}

		private void NotifyIconMouseClick(object sender, MouseEventArgs e)
		{
			if (MouseClick != null) MouseClick(GetMouseButton(e));
		}

		public bool IsVisible
		{
			get { return notifyIcon.Visible; }
			set { notifyIcon.Visible = value; }
		}

		public Icon Icon
		{
			get { return notifyIcon.Icon;}
			set { notifyIcon.Icon = value;}
		}

		public void SetMenuItems(IList<UMenuItem> items, bool stripMenu)
		{
			DisposeMenuItems();
			if (stripMenu)
			{
				notifyIcon.ContextMenuStrip = items.ToWinFormsStripMenu();
			}
			else
			{
				notifyIcon.ContextMenu = items.ToWinFormsMenu();
			}
		}

		private static ToolTipIcon GetIcon(TipIcon icon)
		{
			switch(icon)
			{
				case TipIcon.Info: return ToolTipIcon.Info;
				case TipIcon.Error: return ToolTipIcon.Error;
				case TipIcon.Warning: return ToolTipIcon.Warning;
				default: return ToolTipIcon.None;
			}
		}

		public void ShowBalloonTip(int timeout, string title, string text, TipIcon icon)
		{
			notifyIcon.ShowBalloonTip(timeout, title, text, GetIcon(icon));
		}

		public string HoverText
		{
			get { return notifyIcon.Text; }
			set
			{
				if (value.Length > 63)
				{
					value = value.Substring(0, 63);
				}
				notifyIcon.Text = value;
			}
		}

		public void UpdateSubMenuItems(string name, UMenuItem[] items)
		{
			if (notifyIcon.ContextMenuStrip != null)
			{
				var item = notifyIcon.ContextMenuStrip.Items
				                     .Cast<ToolStripItem>().Single(x => string.Equals(x.Text, name));
				var menu = (ToolStripMenuItem) item;
				foreach (var tmp in menu.DropDownItems.OfType<IDisposable>().ToArray())
				{
					tmp.Dispose();
				}
				menu.DropDownItems.Clear();
				menu.DropDownItems.AddRange(items.ToStripMenuItems());
			}
			else if( notifyIcon.ContextMenu != null)
			{
				var menu = notifyIcon.ContextMenu.MenuItems
										 .Cast<MenuItem>().Single(x => string.Equals(x.Text, name));
				foreach (var tmp in menu.MenuItems.OfType<IDisposable>().ToArray())
				{
					tmp.Dispose();
				}
				menu.MenuItems.Clear();
				menu.MenuItems.AddRange(items.ToMenuItems());
			}
		}

		public void Dispose()
		{
			notifyIcon.Visible = false;
			DisposeMenuItems();
			notifyIcon.Dispose();
		}

		private void DisposeMenuItems()
		{
			if (notifyIcon.ContextMenuStrip != null)
			{
				notifyIcon.ContextMenuStrip.Dispose();
				notifyIcon.ContextMenuStrip = null;
			}
			if (notifyIcon.ContextMenu != null)
			{
				notifyIcon.ContextMenu.Dispose();
				notifyIcon.ContextMenu = null;
			}
		}
	}
}
