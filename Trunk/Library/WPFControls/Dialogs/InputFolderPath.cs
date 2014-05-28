using System.Collections.Generic;
using System.Windows;
using Ookii.Dialogs.Wpf;
using Mnk.Library.WpfWinForms;

namespace Mnk.Library.WpfControls.Dialogs
{
	class InputFolderPath : IDialog
	{
		public bool IsVisible { get; private set; }

		public KeyValuePair<bool, string> ShowDialog(string caption, string path, Window owner)
		{
    	    IsVisible = true;
		    var dialog = new VistaFolderBrowserDialog {ShowNewFolderButton = true};
            try
			{
				dialog.Description = caption;
				if (!string.IsNullOrEmpty(path))
				{
					dialog.SelectedPath = path;
				}
				if (dialog.ShowDialog(owner) == true)
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
