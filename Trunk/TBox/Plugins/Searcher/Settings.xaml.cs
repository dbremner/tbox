using System.Windows.Controls;
using Mnk.TBox.Core.Interface;

namespace Mnk.TBox.Plugins.Searcher
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
