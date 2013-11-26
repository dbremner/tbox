using System.Linq;
using System.Windows;
using LibsLocalization.WPFControls;
using WPFControls.Dialogs;

namespace WPFControls.Controls
{
	public class FilePathGetter : IPathGetter
	{
	    private readonly Window owner;
	    private readonly string caption;
		private readonly string filter;

		public FilePathGetter(Window owner, string caption = null, string filter = null)
		{
		    this.owner = owner;
		    this.filter = filter ?? WPFControlsLang.AllFiles;
			this.caption = caption??WPFControlsLang.SelectFile;
		}

		public bool Get(ref string path)
		{
			var result = DialogsCache.ShowInputFilePath(caption, path, owner, filter);
			if (result.Key)
			{
				path = result.Value.FirstOrDefault();
				return true;
			}
			return false;
		}
	}
}
