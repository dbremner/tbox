using System.Windows;
using System.Windows.Controls;
using FileWatcher.Code;
using Interface;

namespace FileWatcher
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public sealed partial class Settings : ISettings
	{
		public Settings()
		{
			InitializeComponent();
		}

		private void CheckBoxChecked(object sender, RoutedEventArgs e)
		{
			Dirs.OnCheckChangedEvent(sender, e);
		}

		public UserControl Control { get { return this; } }
	}
}
