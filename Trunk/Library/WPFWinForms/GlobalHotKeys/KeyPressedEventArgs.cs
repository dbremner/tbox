using System;

namespace Mnk.Library.WpfWinForms.GlobalHotkeys
{
	public class KeyPressedEventArgs : EventArgs
	{
		public GlobalHotkey GlobalHotkey { get; private set; }
		public KeyPressedEventArgs(GlobalHotkey globalHotkey)
		{
			GlobalHotkey = globalHotkey;
		}
	}
}
