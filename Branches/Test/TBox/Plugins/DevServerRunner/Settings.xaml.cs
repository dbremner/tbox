using System.Windows;
using System.Windows.Controls;
using Interface;

namespace DevServerRunner
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : ISettings
	{
		public Settings()
		{
			InitializeComponent();
		}

		public UserControl Control { get { return this; } }

		private void CheckBoxChecked(object sender, RoutedEventArgs e)
		{
			Dirs.OnCheckChangedEvent(sender,e);
		}
	}
}
