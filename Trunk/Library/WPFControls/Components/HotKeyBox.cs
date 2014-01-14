using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Mnk.Library.WPFWinForms.GlobalHotKeys;

namespace Mnk.Library.WPFControls.Components
{
	public class HotKeyBox : TextBox
	{
		private readonly Microsoft.VisualBasic.Devices.Keyboard userKeyboard = new Microsoft.VisualBasic.Devices.Keyboard();
		private readonly Key[] keysToSkip = new[]{
				                           Key.LeftAlt,Key.RightAlt,
				                           Key.LeftCtrl,Key.RightCtrl,
				                           Key.LeftShift,Key.RightShift,
										   Key.LWin, Key.RWin
			                           };
		private bool isWinPressed = false;

		public HotKeyBox()
		{
			IsReadOnly = true;
		}

		public static readonly DependencyProperty HotKeyProperty =
			DependencyProperty.Register("HotKey", typeof(HotKey),
			typeof(HotKeyBox), new FrameworkPropertyMetadata(new HotKey(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentValuePropertyChanged)
			);
		private static void OnCurrentValuePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			((HotKeyBox)source).HotKey = (HotKey)e.NewValue;
		}

		private HotKey hotKey;
		public HotKey HotKey
		{
			get { return hotKey; } 
			set
			{
				hotKey = value;
				SetValue(HotKeyProperty, value);
				if (hotKey == null)
				{
					Text = string.Empty;
					return;
				}
				Text = hotKey.ToString();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			e.Handled = true;
			if(IsWinPressed(e))isWinPressed = true;
			var system = GetModifiersKeys();
			if(isWinPressed)system |= ModifierKeys.Windows;
			HotKey = new HotKey(system, GetValidKey(e));
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			Background = new SolidColorBrush(Colors.Silver); 
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			Background = new SolidColorBrush(Colors.White); 
			base.OnLostFocus(e);
		}

		private Key GetValidKey(KeyEventArgs e)
		{
			var key = e.Key;
			if (key == Key.System) key = e.SystemKey;
			return Array.IndexOf(keysToSkip, key) != -1 ? Key.None : key;
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			e.Handled = true;
			if (IsWinPressed(e))isWinPressed = false;
		}

		private static bool IsWinPressed(KeyEventArgs e)
		{
			return e.SystemKey == Key.LWin || e.SystemKey == Key.RWin;
		}

		private ModifierKeys GetModifiersKeys()
		{
			var system = ModifierKeys.None;
			if (userKeyboard.AltKeyDown) system |= ModifierKeys.Alt;
			if (userKeyboard.CtrlKeyDown) system |= ModifierKeys.Control;
			if (userKeyboard.ShiftKeyDown) system |= ModifierKeys.Shift;
			return system;
		}
	}
}
