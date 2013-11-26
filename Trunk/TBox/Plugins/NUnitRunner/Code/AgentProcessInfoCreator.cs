using System;
using System.Diagnostics;
using System.IO;

namespace NUnitRunner.Code
{
	class AgentProcessInfoCreator
	{
		private readonly string nunitAgentPath;

		public AgentProcessInfoCreator(string nunitAgentPath)
		{
			this.nunitAgentPath = nunitAgentPath;
		}

		public ProcessStartInfo CreateInfo(string path, int hwnd, string command, bool runAsx86, bool runAsAdmin)
		{
			var fileName = nunitAgentPath;
			var args = string.Format("{0} \"{1}\" {2}", hwnd, path, command);
			if (runAsx86) ApplyCommand("RunAsx86.exe", ref args, ref fileName);
			if (runAsAdmin) ApplyCommand("Tools\\gksudo.exe", ref args, ref fileName);
			return new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = args,
				CreateNoWindow = true,
				UseShellExecute = runAsAdmin,
			};
		}

		private static void ApplyCommand(string command, ref string args, ref string fileName)
		{
			args = string.Format("\"{0}\" {1}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), args);
			fileName = command;
		}
	}
}
