using System.Windows;
using LibsLocalization.WPFControls;
using WPFControls.Dialogs;

namespace WPFControls.Controls
{
	public class FolderPathGetter : IPathGetter
	{
	    private readonly Window owner;
	    private readonly string caption;

        public FolderPathGetter(Window owner, string caption = null)
        {
            this.owner = owner;
            this.caption = caption??WPFControlsLang.SelectFolder;
        }

	    public bool Get(ref string path)
		{
			var result = DialogsCache.ShowInputFolderPath(caption, path, owner);
			if (!result.Key) return false;
			path = result.Value;
			return true;
		}
	}
}