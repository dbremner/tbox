using System;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal class TestsExecutor : ITestsExecutor
    {
        private readonly IReportBuilder reportBuilder;
        private readonly IPackage<IThreadTestConfig> package;
        private readonly IUpdater updater;
        private readonly ILog log = LogManager.GetLogger<TestsExecutor>();

        public TestsExecutor(IReportBuilder reportBuilder, IPackage<IThreadTestConfig> package, IUpdater updater)
        {
            this.reportBuilder = reportBuilder;
            this.package = package;
            this.updater = updater;
        }

        public int Run(CommandLineArgs args)
        {
            var workingDirectory = Environment.CurrentDirectory;
            int retValue = 0;
            var view = new ConsoleView(reportBuilder);
            foreach (var path in args.Paths)
            {
                var testsUpdater = BuildUpdater(args.Labels, args.Teamcity, updater);
                var config = CreateConfig(args, path);
                retValue = Math.Min(retValue, RunTest(args, path, view, config, testsUpdater));
            }

            view.PrintTotalResults();
            PrintTotalInfo(view, args.XmlReport, args.OutputReport, args.Paths.FirstOrDefault(), workingDirectory);
            return retValue;
        }

        private int RunTest(CommandLineArgs args, string path, ConsoleView view, IThreadTestConfig config, ITestsUpdater testsUpdater)
        {
            view.NotifyNewAssemblyStartTest();

            Console.WriteLine("Running tests for {0}.", path);
            if (args.Logo) Console.WriteLine("Calculating tests.");
            if (!package.EnsurePathIsValid(config))
            {
                log.Write("Incorrect path: " + path);
                return -3;
            }

            var results = package.Refresh(config);
            if (args.Logo) Console.WriteLine("{0} tests found.", results.Metrics.Total);
            if (results.IsFailed)
            {
                log.Write("Can't calculate tests count");
                return -3;
            }

            if (args.Logo) Console.WriteLine("Running tests.");
            results = package.Run(config, results, testsUpdater);
            view.SetItems(results);
            if (results.Metrics.FailedCount > 0) return -2;
            return 0;
        }

        private static IThreadTestConfig CreateConfig(CommandLineArgs args, string path)
        {
            return new ThreadTestConfig
            {
                CopyMasks = args.CopyMasks,
                CommandBeforeTestsRun = args.CommandBeforeTestsRun,
                CopyToSeparateFolders = args.Clone,
                DirToCloneTests = args.DirToCloneTests,
                NeedOutput = !string.IsNullOrEmpty(args.OutputReport),
                NeedSynchronizationForTests = args.Sync,
                ProcessCount = args.TestsInParallel,
                ResolveEventHandler = Program.LoadFromSameFolder,
                RuntimeFramework = args.RuntimeFramework,
                StartDelay = args.StartDelay,
                TestDllPath = path,
                OptimizeOrder = args.Prefetch,
                Categories = args.Include ?? args.Exclude,
                IncludeCategories = args.Include != null && args.Exclude != null
            };
        }

        private static SimpleUpdater BuildUpdater(bool labels, bool teamcity, IUpdater updater)
        {
            if(teamcity)return new TeamcityUpdater(updater);
            return labels ? 
                new NUnitLabelsUpdater(updater) : 
                new SimpleUpdater(updater);
        }

        private static void PrintTotalInfo(ConsoleView view, string xmlReport, string outputReport, string path, string dir)
        {
            Directory.SetCurrentDirectory(dir);
            if (!string.IsNullOrEmpty(xmlReport)) view.GenerateReport(path, xmlReport);
            if (!string.IsNullOrEmpty(outputReport))
            {
                var totalResult = view.CreateTotalResult();
                File.WriteAllText(
                    Path.Combine(Environment.CurrentDirectory, outputReport),
                    string.Join(string.Empty, totalResult.Metrics.All.Select(x => x.Output)));
            }
        }
    }
}
