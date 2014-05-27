using System;
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    class AgentProcessCreator
    {
        private readonly string nunitAgentPath;
        private readonly string runAsx86Path;
        private readonly bool runAsx86;
        private readonly bool runAsAdmin;
        private readonly string runtimeFramework;

        public AgentProcessCreator(string nunitAgentPath, string runAsx86Path, bool runAsx86, bool runAsAdmin, string runtimeFramework)
        {
            this.nunitAgentPath = nunitAgentPath;
            this.runAsx86Path = runAsx86Path;
            this.runAsx86 = runAsx86;
            this.runAsAdmin = runAsAdmin;
            this.runtimeFramework = runtimeFramework;
        }

        public IContext Create(string path, string handle, string command)
        {
            var fileName = nunitAgentPath;
            var args = string.Format(CultureInfo.InvariantCulture, "{0} \"{1}\" {2} {3}", handle, path, command, runtimeFramework??string.Empty);
            if (runAsx86) ApplyCommand(Path.Combine(Environment.CurrentDirectory, runAsx86Path), ref args, ref fileName);
            if (!File.Exists(nunitAgentPath)) throw new ArgumentException("Can't find nunit agent: " + nunitAgentPath);
            var pi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = runAsAdmin,
            };
            if (runAsAdmin && !string.Equals(command, TestsCommands.Collect, StringComparison.OrdinalIgnoreCase)) pi.Verb = "runas";
            return new ProcessContext(Process.Start(pi));
        }

        private static void ApplyCommand(string command, ref string args, ref string fileName)
        {
            args = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), args);
            fileName = command;
        }
    }
}
