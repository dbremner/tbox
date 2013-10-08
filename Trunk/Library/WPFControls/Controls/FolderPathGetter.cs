using LibsLocalization.WPFControls;
using WPFControls.Dialogs;

namespace WPFControls.Controls
{
	public class FolderPathGetter : IPathGetter
	{
		private readonly string caption;

        public FolderPathGetter(string caption = null)
		{
			this.caption = caption??WPFControlsLang.SelectFolder;
		}

		public bool Get(ref string path)
		{
			var result = DialogsCache.ShowInputFolderPath(caption, path);
			if (!result.Key) return false;
			path = result.Value;
			return true;
		}
	}
}