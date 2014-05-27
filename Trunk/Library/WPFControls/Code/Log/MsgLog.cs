using System;
using System.Windows;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WPFControls.Code.Log
{
	public class MsgLog : AbstractLog
	{
		public override void Write(string value)
		{
			MessageBox.Show(value, WPFControlsLang.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public override void Write(Exception ex, string value)
		{
			Write(string.Format(WPFControlsLang.ExceptionsTemplate,
				value, Environment.NewLine, ExceptionsHelper.ExpandOnlyMessages(ex)));
		}

	}
}
