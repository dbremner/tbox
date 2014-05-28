using System;

namespace Mnk.Library.WpfControls.Code.Log
{
	public class MultiCaptionedLog : ICaptionedLog
	{
		private readonly ICaptionedLog[] logs;

		public MultiCaptionedLog(ICaptionedLog[] logs)
		{
			this.logs = logs;
		}

		public void Write(string caption, string value)
		{
			foreach (var log in logs)
			{
				log.Write(caption, value);
			}
		}
	}
}
