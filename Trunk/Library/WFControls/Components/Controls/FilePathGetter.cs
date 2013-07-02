using System.Windows.Forms;

namespace WFControls.Components.Controls
{
	public class FilePathGetter : IPathGetter
	{
		private readonly OpenFileDialog m_dialog;

		public FilePathGetter(OpenFileDialog dialog)
		{
			m_dialog = dialog;
		}

		public bool Get(ref string path)
		{
			m_dialog.FileName = path;
			if( m_dialog.ShowDialog() == DialogResult.OK)
			{
				path = m_dialog.FileName;
				return true;
			}
			return false;
		}
	}
}
