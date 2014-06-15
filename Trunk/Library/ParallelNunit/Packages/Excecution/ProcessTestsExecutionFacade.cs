﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ProcessTestsExecutionFacade : ITestsExecutionFacade
    {
        public TestsResults CollectTests(ITestsConfig config, InterprocessServer<INunitRunnerClient> server)
        {
            var client = ((NunitRunnerClient)server.Owner);
            Execute(() => Create(config, server.Handle, TestsCommands.Collect));
            return new TestsResults(client.Collection);
        }

        public virtual void Run(ITestsConfig config, string handle)
        {
            Execute(()=>Create(config, handle, config.NeedSynchronizationForTests ? TestsCommands.Test : TestsCommands.FastTest));
        }

        protected static void Execute(Func<Process> operation)
        {
            Process context = null;
            try
            {
                context = operation();
            }
            finally
            {
                if (context != null)
                {
                    context.WaitForExit();
                    context.Dispose();
                }
            }
        }

        protected static Process Create(ITestsConfig config, string handle, string command)
        {
            var fileName = config.NunitAgentPath;
            var args = string.Format(CultureInfo.InvariantCulture, "{0} \"{1}\" {2} {3}", handle, config.TestDllPath, command, config.RuntimeFramework ?? string.Empty);
            if (config.RunAsx86) ApplyCommand(Path.Combine(Environment.CurrentDirectory, config.RunAsx86Path), ref args, ref fileName);
            if (!File.Exists(config.NunitAgentPath)) throw new ArgumentException("Can't find nunit agent: " + config.NunitAgentPath);
            var pi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = config.RunAsAdmin,
            };
            if (config.RunAsAdmin && !string.Equals(command, TestsCommands.Collect, StringComparison.OrdinalIgnoreCase)) pi.Verb = "runas";
            return Process.Start(pi);
        }

        private static void ApplyCommand(string command, ref string args, ref string fileName)
        {
            args = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), args);
            fileName = command;
        }

    }
}