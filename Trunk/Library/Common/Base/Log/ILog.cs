using System;

namespace Mnk.Library.Common.Base.Log
{
	public interface ILog : IBaseLog
	{
		void Write(string format, params object[] args);
		void Write(Exception ex, string format, params object[] args);
	}
}
