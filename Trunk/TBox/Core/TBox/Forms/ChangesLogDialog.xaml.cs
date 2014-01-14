using System.Windows;
using Mnk.TBox.Locales.Localization.TBox;

namespace Mnk.TBox.Core.Application.Forms
{
	/// <summary>
	/// Interaction logic for ChangesLogDialog.xaml
	/// </summary>
	public sealed partial class ChangesLogDialog
	{
		public ChangesLogDialog()
		{
			InitializeComponent();
			Title = TBoxLang.ChangeLogTitle;
		}

		public void ShowDialog(string message)
		{
			Message.Text = message;
			ShowAndActivate();
		}

		private void CloseClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
