using System;
using System.Globalization;
using System.Windows;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Code.Log
{
	public class MessageBoxLog : AbstractLog
	{
		public override void Write(string value)
		{
			MessageBox.Show(value, WPFControlsLang.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public override void Write(Exception ex, string value)
		{
            Write(string.Format(CultureInfo.InvariantCulture, WPFControlsLang.ExceptionsTemplate,
				value, Environment.NewLine, ExceptionsHelper.ExpandOnlyMessages(ex)));
		}

	}
}
