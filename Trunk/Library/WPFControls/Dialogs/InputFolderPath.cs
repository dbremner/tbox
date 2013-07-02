using System.Collections.Generic;
using System.Windows;
using WPFWinForms;

namespace WPFControls.Dialogs
{
	class InputFolderPath : IDialog
	{
		private readonly OpenFolderDialog dialog = new OpenFolderDialog{ShowNewFolderButton = true};

		public void Dispose()
		{
			dialog.Dispose();
		}

		public bool IsVisible { get; private set; }

		public KeyValuePair<bool, string> ShowDialog(string caption, string path, Window owner = null)
		{
			try
			{
				IsVisible = true;
				dialog.Description = caption;
				if (!string.IsNullOrEmpty(path))
				{
					dialog.SelectedPath = path;
				}
				if (dialog.ShowDialog() == true)
				{
					return new KeyValuePair<bool, string>(true, dialog.SelectedPath);
				}
			}
			finally
			{
				IsVisible = false;
			}
			return new KeyValuePair<bool, string>(false, string.Empty);
		}
	}
}
