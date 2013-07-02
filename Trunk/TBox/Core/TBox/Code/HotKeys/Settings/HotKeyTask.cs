using System;
using Common.UI.Model;
using WPFWinForms.GlobalHotKeys;

namespace TBox.Code.HotKeys.Settings
{
	[Serializable]
	public class HotKeyTask : CheckableData
	{
		public HotKey HotKey { get; set; }

		public override object Clone()
		{
			return new HotKeyTask
				{
					Key = Key,
					IsChecked = IsChecked,
					HotKey = (HotKey == null) ? 
								null : new HotKey(HotKey.Modifier, HotKey.Key)
				};
		}

		public override string ToString()
		{
			return Key.Replace(Environment.NewLine, "/");
		}
	}
}
