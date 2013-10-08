using System.Linq;
using LibsLocalization.WPFControls;
using WPFControls.Dialogs;

namespace WPFControls.Controls
{
	public class FilePathGetter : IPathGetter
	{
		private readonly string caption;
		private readonly string filter;

		public FilePathGetter(string caption = null, string filter = null)
		{
			this.filter = filter ?? WPFControlsLang.AllFiles;
			this.caption = caption??WPFControlsLang.SelectFile;
		}

		public bool Get(ref string path)
		{
			var result = DialogsCache.ShowInputFilePath(caption, path, filter);
			if (result.Key)
			{
				path = result.Value.FirstOrDefault();
				return true;
			}
			return false;
		}
	}
}
