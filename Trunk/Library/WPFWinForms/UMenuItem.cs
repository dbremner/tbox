using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mnk.Library.WpfWinForms
{
	public class UMenuItem
	{
		public string Header { get; set; }
		public string Hotkey { get; set; }
		public Action<object> OnClick { get; set; }
		public IList<UMenuItem> Items { get; set; }
		public bool IsEnabled { get; set; }
		public Icon Icon { get; set; }

		public UMenuItem()
		{
			IsEnabled = true;
			Items = new List<UMenuItem>();
		}
	}
}
