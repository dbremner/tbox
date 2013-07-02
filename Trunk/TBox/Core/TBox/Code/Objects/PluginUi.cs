using System;
using System.Drawing;
using System.Windows.Controls;
using WPFWinForms;

namespace TBox.Code.Objects
{
	class PluginUi
	{
		public string Key { get; set; }
		public string Name { get; set; }
		public Icon Icon { get; set; }
		public UMenuItem[] Menu { get; set; }
		public Func<Control> Settings { get; set; }
	}
}
