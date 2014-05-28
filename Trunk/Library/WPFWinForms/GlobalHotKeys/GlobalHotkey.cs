using System;
using System.Windows.Input;

namespace Mnk.Library.WpfWinForms.GlobalHotkeys
{
	[Serializable]
	public class GlobalHotkey
	{
		public ModifierKeys Modifier { get; set; }
		public Key Key { get; set; }
		public GlobalHotkey(ModifierKeys modifier, Key key)
		{
			Modifier = modifier;
			Key = key;
		}
		public GlobalHotkey(){}
		public override string ToString()
		{
			var key = GetKeyText(Key);
			var modifier = (Modifier == ModifierKeys.None) ?
				string.Empty : Modifier.ToString().Replace("Control", "Ctrl");
			if (!string.IsNullOrEmpty(key))
			{
				if (!string.IsNullOrEmpty(modifier))
				{
					return modifier + " + " + key;
				}
				return key;
			}
			return string.Empty;
		}

		private static string GetKeyText(Key key)
		{
			if (key == Key.None) return string.Empty;
			var str = key.ToString();
			if (key >= Key.D0 && key <= Key.D9)
				return str.Substring(1);
			return str;
		}
	}
}
