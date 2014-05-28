using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;

namespace Mnk.Library.WpfControls.Dialogs
{
	class InputFilePath : IDialog
	{
		private readonly OpenFileDialog dialog = new OpenFileDialog{CheckFileExists = true};

		public void Dispose()
		{
		}

		public bool IsVisible { get; private set; }

        public KeyValuePair<bool, string[]> ShowDialog(string caption, string path, string filter, Window owner, bool allowSelectMany = false)
		{
			try
			{
				IsVisible = true;
				dialog.Title = caption;
				dialog.Filter = filter;
				if (!string.IsNullOrEmpty(path))
				{
					dialog.FileName = path;
				}
				dialog.Multiselect = allowSelectMany;
				if (dialog.ShowDialog(owner) == true)
				{
					return new KeyValuePair<bool, string[]>(true, dialog.FileNames);
				}
			}
			finally
			{
				IsVisible = false;
			}
			return new KeyValuePair<bool, string[]>(false, new string[0]);
		} 
	}
}
