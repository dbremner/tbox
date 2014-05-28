using System;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfWinForms.GlobalHotkeys;

namespace Mnk.TBox.Core.Application.Code.HotKeys.Settings
{
	[Serializable]
	public class HotKeyTask : CheckableData
	{
		public GlobalHotkey GlobalHotkey { get; set; }

		public override object Clone()
		{
			return new HotKeyTask
				{
					Key = Key,
					IsChecked = IsChecked,
					GlobalHotkey = (GlobalHotkey == null) ? 
								null : new GlobalHotkey(GlobalHotkey.Modifier, GlobalHotkey.Key)
				};
		}

		public override string ToString()
		{
			return Key.Replace(Environment.NewLine, "/");
		}
	}
}
