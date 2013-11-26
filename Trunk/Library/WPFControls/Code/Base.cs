using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFControls.Code
{
	public static class Base
	{
		public static MenuItem CreateMenuItem(string header, Action<object, RoutedEventArgs> onClick)
		{
			var menu = new MenuItem{ Header = header };
			if (onClick != null) menu.Click += new RoutedEventHandler(onClick);
			return menu;
		}
	}
}
