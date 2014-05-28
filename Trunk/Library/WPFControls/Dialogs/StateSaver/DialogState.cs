using System;
using System.Windows;

namespace Mnk.Library.WpfControls.Dialogs.StateSaver
{
	[Serializable]
	public class DialogState
	{
		public double Left { get; set; }
		public double Top { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public WindowState WindowState { get; set; }
	}
}
