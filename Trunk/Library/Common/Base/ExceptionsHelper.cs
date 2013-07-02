using System;
using System.Text;
using Common.Base.Log;

namespace Common.Base
{
	public static class ExceptionsHelper
	{
		public static string Expand(Exception ex)
		{
			return SplitMessages(ex)
				.AppendLine(ex.StackTrace)
				.ToString();
		}

		public static string ExpandOnlyMessages(Exception ex)
		{
			return SplitMessages(ex).ToString();
		}

		private static StringBuilder SplitMessages(Exception ex)
		{
			var sb = new StringBuilder(ex.Message + Environment.NewLine);
			while ((ex = ex.InnerException) != null)
			{
				sb.AppendLine(ex.Message);
			}
			return sb;
		}

		public static bool HandleException(Action action, Action<Exception> onException)
		{
			try
			{
				action();
				return false;
			}
			catch (Exception ex)
			{
				onException(ex);
			}
			return true;
		}

		public static bool HandleException(Action action, Func<string> messageGetter, ILog log)
		{
			return HandleException(action, ex => log.Write(ex, messageGetter()));
		}
	}
}
