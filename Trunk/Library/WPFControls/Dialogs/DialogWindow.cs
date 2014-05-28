using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Dialogs
{
	public class ClosableDialogWindow : Window
	{
        public ClosableDialogWindow()
		{
			ShowActivated = true;
			ShowInTaskbar = false;
			SnapsToDevicePixels = true;
			SetResourceReference(BackgroundProperty, SystemColors.ControlBrushKey);
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
		}
	}
}
