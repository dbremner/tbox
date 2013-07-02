using System;
using System.Windows;
using Common.Base;
using Common.Base.Log;

namespace WPFControls.Code.Log
{
	public class MsgLog : AbstractLog
	{
		public override void Write(string value)
		{
			MessageBox.Show(value, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public override void Write(Exception ex, string value)
		{
			Write(string.Format("{0}{1}Exceptions:{1}{2}",
				value, Environment.NewLine, ExceptionsHelper.ExpandOnlyMessages(ex)));
		}

	}
}
