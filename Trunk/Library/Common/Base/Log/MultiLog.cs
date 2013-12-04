using System;

namespace Common.Base.Log
{
	public class MultiLog : AbstractLog
	{
		private readonly IBaseLog[] logs;

		public MultiLog(params IBaseLog[] logs)
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
