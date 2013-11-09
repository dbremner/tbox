using System.Diagnostics;
using System.IO;
using Common.Base;
using Common.Base.Log;

namespace Common.Console
{
	public static class Cmd
	{
		public static bool Start(string path, ILog log, string args = null, bool waitEnd = true, bool nowindow = false, string directory=null)
		{
            if (string.IsNullOrEmpty(path))
            {
                log.Write("Can't execute executable with empty path, please specify path");
                return false;
            }
            if (string.IsNullOrEmpty(directory)) directory = Path.GetDirectoryName(path) ?? string.Empty;
			return ExceptionsHelper.HandleException(
				() => DoStart(path, args ?? string.Empty, directory, waitEnd, nowindow),
				() => "Can't execute: " + path,
				log
				);
		}

		private static void DoStart(string path, string args, string workingDirectory, bool waitEnd, bool nowindow)
		{
			var info = new ProcessStartInfo
			{
				FileName = path,
				WorkingDirectory = workingDirectory,
				Arguments = args,
				CreateNoWindow = nowindow,
				UseShellExecute = false,
			};
			using (var proc = Process.Start(info))
			{
				if (waitEnd)
				{
					proc.WaitForExit();
				}
			}
		}
	}
}
