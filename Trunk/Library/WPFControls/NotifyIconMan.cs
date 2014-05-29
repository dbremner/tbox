using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.Library.WpfControls
{
	public sealed class NotifyIconMan : IDisposable
	{
		public TrayIcon NotifyIcon { get; set; }
		private readonly Window owner;
		private readonly Action<object> onDoubleClick;

		public NotifyIconMan( Window owner, Icon icon, Action<object> onDoubleClick = null)
		{
			this.owner = owner;
			this.onDoubleClick = onDoubleClick;
			this.owner.Closed += Closed;
			this.owner.StateChanged += StateChanged;
			NotifyIcon = new TrayIcon {Icon = icon, IsVisible = true};
			NotifyIcon.DoubleClick += OnDoubleClick;
		}

		private void OnDoubleClick(object sender, EventArgs e)
		{
			if (onDoubleClick != null)
			{
				onDoubleClick(this);
				return;
			}
			if(!owner.IsVisible)owner.Show();
			if (owner.WindowState == WindowState.Minimized)
			{
				owner.WindowState = WindowState.Normal;
			}
			owner.Activate();
		}

		public void SetMenuItems(IList<UMenuItem> items, bool stripMenu)
		{
			NotifyIcon.SetMenuItems(items, stripMenu);
		}

		private void StateChanged( object sender, EventArgs eventArgs)
		{
			if ( owner.WindowState == WindowState.Minimized )
			{
				owner.Hide();
			}
		}

		private void Closed( object sender, EventArgs e )
		{
			Dispose();
		}

		public void Dispose()
		{
			NotifyIcon.Dispose();
		}

		public void UpdateSubMenuItems(string name, UMenuItem[] items)
		{
			NotifyIcon.UpdateSubMenuItems(name, items);
		}
	}
}
