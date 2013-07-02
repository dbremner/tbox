using WPFControls.Dialogs;

namespace WPFControls.Controls
{
	public class FolderPathGetter : IPathGetter
	{
		private readonly string caption;

        public FolderPathGetter(string caption = "Select folder")
		{
			this.caption = caption;
		}

		public bool Get(ref string path)
		{
			var result = DialogsCache.ShowInputFolderPath(caption, path);
			if( result.Key )
			{
				path = result.Value;
				return true;
			}
			return false;
		}
	}
}