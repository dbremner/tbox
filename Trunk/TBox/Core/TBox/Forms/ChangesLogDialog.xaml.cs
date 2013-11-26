using System.Windows;
using Localization.TBox;

namespace TBox.Forms
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
