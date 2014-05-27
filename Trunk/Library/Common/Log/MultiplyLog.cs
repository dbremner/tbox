using System;

namespace Mnk.Library.Common.Log
{
	public class MultiplyLog : AbstractLog
	{
		private readonly IBaseLog[] logs;

		public MultiplyLog(params IBaseLog[] logs)
		{
			this.logs = logs;
		}

		public override void Write(string value)
		{
			foreach (var log in logs)
			{
				log.Write(value);
			}
		}

		public override void Write(Exception ex, string value)
		{
			foreach (var log in logs)
			{
				log.Write(ex, value);
			}
		}
	}
}
