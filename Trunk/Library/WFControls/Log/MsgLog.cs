using System.Windows.Forms;
using Common.Base.Log;

namespace WFControls.Log
{
	public class MsgLog : AbstractLog
	{
		public override void Write(string value)
		{
			MessageBox.Show(value, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
