using System;

namespace Common.Base.Log
{
	public abstract class AbstractLog : ILog
	{
		public abstract void Write(string value);
		public virtual void Write(Exception ex, string value)
		{
			Write(string.Format("{0}{1}{2}", value, Environment.NewLine, ex));
		}

		public void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		public void Write(Exception ex, string format, params object[] args)
		{
			Write(ex, string.Format(format, args));
		}
	}
}
