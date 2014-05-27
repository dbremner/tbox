using System;

namespace Mnk.Library.Common.Log
{
	public interface IBaseLog
	{
		void Write(string value);
		void Write(Exception ex, string value);
	}
}
