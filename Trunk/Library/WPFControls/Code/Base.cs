using System;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Code
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
