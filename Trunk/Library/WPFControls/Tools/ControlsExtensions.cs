using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFWinForms.Icons;

namespace WPFControls.Tools
{
	public static class ControlsExtensions
	{
		public static Window GetParentWindow(this DependencyObject child)
		{
			var parentObject = VisualTreeHelper.GetParent(child);
			if (parentObject == null)
			{
				return null;
			}
			var parent = parentObject as Window;
			return parent ?? GetParentWindow(parentObject);
		}

		public static Window GetParentWindow(UserControl child)
		{
			return GetParentWindow((DependencyObject)child);
		}

		public static void SetFocus(this Control ctrl)
		{
			if (!ctrl.IsFocused) 
				ctrl.Parent.Dispatcher.BeginInvoke(new Func<bool>(ctrl.Focus));
		}

		public static void SetIcon(this Window ctrl, Icon icon)
		{
			ctrl.Icon = icon.ToImageSource();
		}

		public static void SetVisibility(this Control control, bool state)
		{
			control.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
