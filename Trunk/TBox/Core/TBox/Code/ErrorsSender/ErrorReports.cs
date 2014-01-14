using System;

namespace Mnk.TBox.Core.Application.Code.ErrorsSender
{
	[Serializable]
	public class ErrorReports
	{
		public bool AllowSend { get; set; }
		public string Directory { get; set; }

		public ErrorReports()
		{
			AllowSend = false;
			Directory = @"\\tboxserver\shared\Logs\";
		}
	}
}
