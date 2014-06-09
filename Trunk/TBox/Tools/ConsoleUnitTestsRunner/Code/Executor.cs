using System;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    class Executor : IExecutor
    {
        private readonly ILog log = LogManager.GetLogger<Executor>();
        private readonly IInfoView view;
        private readonly ITestsExecutor executor;

        public Executor(IInfoView view, ITestsExecutor executor)
        {
            this.view = view;
            this.executor = executor;
        }

        public int Execute(string[] args)
        {
            if (args.Length <= 0 || string.Equals(args[0], "/?") || string.Equals(args[0], "/help", StringComparison.OrdinalIgnoreCase))
            {
                view.ShowHelp();
                return -1;
            }

            var ret = -1;
            var wait = false;
            try
            {
                var cmdArgs = new CommandLineArgs(args);
                wait = cmdArgs.Wait;
                if (cmdArgs.Logo)
                {
                    view.ShowLogo();
                    view.ShowArgs(cmdArgs);
                }
                if (!cmdArgs.Paths.Any())
                {
                    view.ShowHelp();
                    return -1;
                }
                var notExist = cmdArgs.Paths.Where(x => !File.Exists(x)).ToArray();
                if (notExist.Any())
                {
                    log.Write("Can't find files: " + string.Join(" ", notExist));
                    return -2;
                }

                Console.WriteLine("ProcessModel: Default\tDomainUseage: Single");
                Console.WriteLine("Execution Runtime: Default\tCPUCount: " + Environment.ProcessorCount);

                ret = executor.Run(cmdArgs);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Internal error");
                view.ShowHelp();
            }
            finally
            {
                NUnitBase.Dispose();
            }

            if (wait) Console.ReadKey();
            return ret;
        }
    }
}
