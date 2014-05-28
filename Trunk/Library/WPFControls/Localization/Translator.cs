using System;
using System.Globalization;
using System.Threading;

namespace Mnk.Library.WpfControls.Localization
{
	public static class Translator
	{
		//internal static event EventHandler CultureChanged;
		public static CultureInfo Culture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				Thread.CurrentThread.CurrentUICulture = value;
				Thread.CurrentThread.CurrentCulture = value;
				//if (CultureChanged != null)CultureChanged(null, EventArgs.Empty);
			}
		}
	}
}
