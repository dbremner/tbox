using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Base.Log;
using Common.Communications.Interprocess;
using ParallelNUnit.Core;
using ParallelNUnit.Execution;
using ParallelNUnit.Infrastructure.Runners;
using ParallelNUnit.Interfaces;
using ServiceStack.Text;

namespace NUnitAgent
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
            LogManager.Init(new MultiLog(new ConsoleLog()));
            var log = LogManager.GetLogger<Program>();
            Console.WriteLine("Execute NUnitAgent with arguments: " + string.Join("; ", args));
            if (args.Length != 3)
            {
                log.Write("You should specify 3 parameters: named pipes handle, path to test dll, method (collect, test, fasttest)");
                return -1;
            }
            var handle = args[0];
            var path = args[1];
            try
            {
                var w = new NUnitExecutor(LoadFromSameFolder);
                switch (args[2])
                {
                    case TestsCommands.Collect:
                        Result list = null;
                        try
                        {
                            list = w.CollectTests(path);
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
                        return w.RunTests(handle, true, true);
                    case TestsCommands.Test:
                        return w.RunTests(handle, false, true);
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
            return (from dir in new[] { "Libraries", "Localization" } select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) into assemblyPath where File.Exists(assemblyPath) select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
        }
    }
}
