using System.Linq;
using WPFControls.Dialogs;

namespace WPFControls.Controls
{
	public class FilePathGetter : IPathGetter
	{
		private readonly string caption;
		private readonly string filter;

		public FilePathGetter(string caption = "Select file", string filter = "All files (*.*)|*.*")
		{
			this.filter = filter;
			this.caption = caption;
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
