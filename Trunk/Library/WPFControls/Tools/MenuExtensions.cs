using System;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Tools
{
	public static class MenuExtensions
	{
		public static MenuItem CreateMenuItem(string header, Action<object, RoutedEventArgs> onClick)
		{
			var menu = new MenuItem{ Header = header };
			if (onClick != null) menu.Click += new RoutedEventHandler(onClick);
			return menu;
		}
	}
}
