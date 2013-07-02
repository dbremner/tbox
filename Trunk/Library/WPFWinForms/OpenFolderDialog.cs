using System;
using System.Windows.Forms;

namespace WPFWinForms
{
	public sealed class OpenFolderDialog : IDisposable
	{
		private readonly FolderBrowserDialog dialog = new FolderBrowserDialog();

		public OpenFolderDialog()
		{
			dialog.ShowNewFolderButton = true;
			dialog.RootFolder = Environment.SpecialFolder.MyComputer;
		}

		public string Description
		{
			get { return dialog.Description; }
			set { dialog.Description = value; }
		}

		public string SelectedPath
		{
			get { return dialog.SelectedPath; }
			set { dialog.SelectedPath = value; }
		}

		public bool ShowNewFolderButton
		{
			get { return dialog.ShowNewFolderButton; }
			set { dialog.ShowNewFolderButton = value; }
		}

		public bool? ShowDialog()
		{
			return dialog.ShowDialog() == DialogResult.OK;
		}

		public void Dispose()
		{
			dialog.Dispose();
		}
	}
}
