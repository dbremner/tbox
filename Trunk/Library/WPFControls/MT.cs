using System;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls
{
	public static class Mt
	{
		public static void Do(Control ctrl, Action a)
		{
			if (!ctrl.Dispatcher.CheckAccess()) ctrl.Dispatcher.Invoke(a);
			else a();
		}
		public static void SetText(TextBox ctrl, string text) { Do(ctrl, () => { ctrl.Text = text; }); }
		public static void SetText(ContentControl ctrl, string text) { Do(ctrl, () => { ctrl.Content = text; }); }
		public static void SetEnabled(Control ctrl, bool state) { Do(ctrl, () => { ctrl.IsEnabled = state; }); }
		public static void SetProgress(ProgressBar pb, float value) { Do(pb, () => { pb.Value = value; }); }
	}
}
