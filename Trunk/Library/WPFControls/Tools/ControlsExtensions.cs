using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Tools
{
	public static class ControlsExtensions
	{
		public static Window GetParentWindow(this DependencyObject child)
		{
			while (true)
			{
				var parentObject = VisualTreeHelper.GetParent(child);
				if (parentObject == null)
				{
					return null;
				}
				var parent = parentObject as Window;
				if (parent != null) return parent;
				child = parentObject;
			}
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

		public static void SetVisibility(this Control control, bool state)
		{
			control.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
