using System;
using System.Diagnostics;
using System.IO;

namespace PluginsShared.UnitTests
{
	class AgentProcessCreator
	{
		private readonly string nunitAgentPath;
	    private readonly string runAsx86Path;

	    public AgentProcessCreator(string nunitAgentPath, string runAsx86Path)
        {
            this.nunitAgentPath = nunitAgentPath;
            this.runAsx86Path = runAsx86Path;
        }

	    public Process Create(string path, string handle, string command, bool runAsx86, bool runAsAdmin)
		{
			var fileName = nunitAgentPath;
			var args = string.Format("{0} \"{1}\" {2}", handle, path, command);
            if (runAsx86) ApplyCommand(Path.Combine(Environment.CurrentDirectory, runAsx86Path), ref args, ref fileName);
			var pi = new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = args,
				CreateNoWindow = true,
				UseShellExecute = runAsAdmin,
			};
			if (runAsAdmin) pi.Verb = "runas";
			try
			{
				return Process.Start(pi);
			}
			catch (Exception)
			{
				pi.Verb = null;
				return Process.Start(pi);
			}
		}

		private static void ApplyCommand(string command, ref string args, ref string fileName)
		{
			args = string.Format("\"{0}\" {1}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), args);
			fileName = command;
		}
	}
}
