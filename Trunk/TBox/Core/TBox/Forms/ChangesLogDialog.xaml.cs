using System;
using System.Windows;

namespace TBox.Forms
{
	/// <summary>
	/// Interaction logic for ChangesLogDialog.xaml
	/// </summary>
	public partial class ChangesLogDialog : IDisposable
	{
		public ChangesLogDialog()
		{
			InitializeComponent();
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
