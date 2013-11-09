using System;

namespace Common.Base.Log
{
	public interface IBaseLog
	{
		void Write(string value);
		void Write(Exception ex, string value);
	}
}
