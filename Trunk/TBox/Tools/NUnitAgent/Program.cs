﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Packages.Excecution;
using Mnk.TBox.Core.Contracts;
using ServiceStack.Text;

namespace Mnk.TBox.Tools.NUnitAgent
{
    class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadFromSameFolder;
        }

        [STAThread]
        static int Main(string[] args)
        {
            LogManager.Init(new MultiplyLog(new ConsoleLog(), new FileLog(Path.Combine(Folders.UserLogsFolder, "NUnitAgent.log"))));
            var log = LogManager.GetLogger<Program>();
            Console.WriteLine("Execute NUnitAgent with arguments: " + string.Join("; ", args));
            if (args.Length < 3 || args.Length > 4)
            {
                log.Write("You should specify 3 or 4 parameters: named pipes handle, path to test dll, method (collect, test, fasttest), [framework]");
                return -1;
            }
            var handle = args[0];
            var path = args[1];
            var framework = args.Length == 4 ? args[3] : null;
            try
            {
                var config = new ThreadTestConfig
                {
                    TestDllPath = path,
                    RuntimeFramework = framework,
                    ResolveEventHandler = LoadFromSameFolder
                };
                var executor = new ThreadTestsExecutor();
                switch (args[2])
                {
                    case TestsCommands.Collect:
                        Result list = null;
                        try
                        {
                            list = executor.CollectTests(config);
                            return 1;
                        }
                        finally
                        {
                            using (var cl = new InterprocessClient<INunitRunnerClient>(handle))
                            {
                                cl.Instance.SetCollectedTests(JsonSerializer.SerializeToString(list));
                            }
                        }
                    case TestsCommands.FastTest:
                        return executor.RunTests(config, handle);
                    case TestsCommands.Test:
                        return executor.RunTests(config, handle);
                    default:
                        log.Write("Unknown command: " + args[2]);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected error");
            }
            finally
            {
                NUnitBase.Dispose();
            }
            return -1;
        }

        static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            return (from dir in new[] { "Libraries", "Localization" } 
                    select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) 
                    into assemblyPath where File.Exists(assemblyPath) 
                    select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
        }
    }
}
