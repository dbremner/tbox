using System.Windows.Controls;
using Interface;

namespace LeaksInterceptor
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
	}
}
