using System;

namespace WPFWinForms.GlobalHotKeys
{
	public class KeyPressedEventArgs : EventArgs
	{
		public HotKey HotKey { get; private set; }
		public KeyPressedEventArgs(HotKey hotKey)
		{
			HotKey = hotKey;
		}
	}
}
