using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Mnk.Library.WpfWinForms.GlobalHotkeys;

namespace Mnk.Library.WpfControls.Components
{
	public class HotkeyBox : TextBox
	{
		private readonly Microsoft.VisualBasic.Devices.Keyboard userKeyboard = new Microsoft.VisualBasic.Devices.Keyboard();
		private readonly Key[] keysToSkip = new[]{
				                           Key.LeftAlt,Key.RightAlt,
				                           Key.LeftCtrl,Key.RightCtrl,
				                           Key.LeftShift,Key.RightShift,
										   Key.LWin, Key.RWin
			                           };
		private bool isWinPressed = false;

		public HotkeyBox()
		{
			IsReadOnly = true;
		}

		public static readonly DependencyProperty GlobalHotkeyProperty =
			DependencyProperty.Register("GlobalHotkey", typeof(GlobalHotkey),
			typeof(HotkeyBox), new FrameworkPropertyMetadata(new GlobalHotkey(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentValuePropertyChanged)
			);
		private static void OnCurrentValuePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			((HotkeyBox)source).GlobalHotkey = (GlobalHotkey)e.NewValue;
		}

		private GlobalHotkey globalHotkey;
		public GlobalHotkey GlobalHotkey
		{
			get { return globalHotkey; } 
			set
			{
				globalHotkey = value;
				SetValue(GlobalHotkeyProperty, value);
				if (globalHotkey == null)
				{
					Text = string.Empty;
					return;
				}
				Text = globalHotkey.ToString();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			e.Handled = true;
			if(IsWinPressed(e))isWinPressed = true;
			var system = GetModifiersKeys();
			if(isWinPressed)system |= ModifierKeys.Windows;
			GlobalHotkey = new GlobalHotkey(system, GetValidKey(e));
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
