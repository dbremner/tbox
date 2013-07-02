using System.Windows.Forms;

namespace WFControls.Components.Controls
{
	public class FolderPathGetter : IPathGetter
	{
		private readonly FolderBrowserDialog m_dialog;

		public FolderPathGetter(FolderBrowserDialog dialog)
		{
			m_dialog = dialog;
		}

		public bool Get(ref string path)
		{
			m_dialog.SelectedPath = path;
			if( m_dialog.ShowDialog() == DialogResult.OK)
			{
				path = m_dialog.SelectedPath;
				return true;
			}
			return false;
		}
	}
}
