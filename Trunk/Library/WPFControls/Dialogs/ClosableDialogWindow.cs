﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WPFControls.Dialogs
{
	public class DialogWindow : Window, IDisposable
	{
		protected bool CanClose { get; set; }

		public DialogWindow()
		{
			CanClose = false;
			ShowActivated = true;
			ShowInTaskbar = false;
			SnapsToDevicePixels = true;
			SetResourceReference(BackgroundProperty, SystemColors.ControlBrushKey);
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!CanClose)
			{
				e.Cancel = true;
				Hide();
			}
			base.OnClosing(e);
		}

		protected virtual void OnShow()
		{
			
		}

		public void SafeShowDialog()
		{
			if (!IsVisible)
			{
				OnShow();
				ShowDialog();
			}
			SafeActivate();
		}

		public void ShowAndActivate()
		{
			if (!IsVisible)
			{
				OnShow();
				Show();
			}
			SafeActivate();
		}

		private void SafeActivate()
		{
			if (WindowState == WindowState.Minimized)
				WindowState = WindowState.Normal;
			Activate();
		}

		public virtual void Dispose()
		{
			CanClose = true;
			Close();
		}
	}
}
