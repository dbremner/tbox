using System;
using System.Windows;
using Common.Base;
using Common.Base.Log;
using LibsLocalization.WPFControls;

namespace WPFControls.Code.Log
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
