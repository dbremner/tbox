using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;
using ExecutionContext = Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts.ExecutionContext;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal class TestsExecutor : ITestsExecutor
    {
        private readonly IReportBuilder reportBuilder;
        private readonly IUpdater updater;
        private readonly static ILog Log = LogManager.GetLogger<TestsExecutor>();

        public TestsExecutor(IReportBuilder reportBuilder, IUpdater updater)
        {
            this.reportBuilder = reportBuilder;
            this.updater = updater;
        }

        public int Run(CommandLineArgs args)
        {
            var workingDirectory = Environment.CurrentDirectory;
            var view = new ConsoleView(reportBuilder);

            if (args.Logo) Console.WriteLine("Calculating tests.");
            var assemblies = CollectTests(args.Paths, args);
            if (assemblies.All(x => x.RetValue == 0))
            {
                var totalResults = new TestsResults(assemblies.SelectMany(x => x.Results.Items).ToArray());
                if (args.Logo)
                {
                    Console.WriteLine("{0} tests found.", totalResults.Metrics.Total);
                    Console.WriteLine("Running tests.");
                }
                var testsUpdater = BuildUpdater(args, updater, totalResults.Metrics.Total);

                if (args.AssembliesInParallel > 1)
                {
                    Parallel.ForEach(assemblies,
                        new ParallelOptions {MaxDegreeOfParallelism = args.AssembliesInParallel}, 
                        assembly => RunTest(assembly, view, testsUpdater, args)
                        );
                }
                else
                {
                    foreach (var assembly in assemblies)
                    {
                        RunTest(assembly,  view, testsUpdater, args);
                    }
                }

                view.PrintTotalResults();
                PrintTotalInfo(view, args.XmlReport, args.OutputReport, args.Paths.FirstOrDefault(), workingDirectory);
            }
            foreach (var assembly in assemblies)
            {
                assembly.Container.Dispose();
            }
            return assemblies.Min(x=>x.RetValue);
        }

        private static ExecutionContext[] CollectTests(IEnumerable<string> paths, CommandLineArgs args)
        {
            return ((args.AssembliesInParallel>1) ? 
                paths.AsParallel().Select(x => Collect(x, args)) :
                paths.Select(x => Collect(x, args))
                ).ToArray()
                ;
        }

        private static ExecutionContext Collect(string path, CommandLineArgs args)
        {
            var context = new ExecutionContext
            {
                Path = path,
                Config = CreateConfig(args, path),
                RetValue = 0,
                Container = Library.ParallelNUnit.ServicesRegistrar.Register(),
            };
            context.Package = context.Container.GetInstance<IPackage<IThreadTestConfig>>();
            if (!context.Package.EnsurePathIsValid(context.Config))
            {
                Log.Write("Incorrect path: " + path);
                context.RetValue = -3;
            }
            else
            {
                context.Results = context.Package.Refresh(context.Config);
                if (context.Results.IsFailed)
                {
                    Log.Write("Can't calculate tests count");
                    context.RetValue = -3;
                }
            }
            return context;
        }

        private static void RunTest(ExecutionContext context, ConsoleView view, ITestsUpdater testsUpdater, CommandLineArgs args)
        {
            var time = Environment.TickCount;
            context.Results = context.Package.Run(context.Config, context.Results, testsUpdater);
            view.SetItems(context.Results);
            if (context.Results.Metrics.FailedCount > 0)
            {
                context.RetValue = -2;
            }
            if (args.Logo && !args.Labels)
            {
                Console.WriteLine("'{0}' is done, time: {1}", Path.GetFileName(context.Path), ((Environment.TickCount - time)/1000).FormatTimeInSec());
            }
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

        private static ITestsUpdater BuildUpdater(CommandLineArgs args, IUpdater updater, int totalCount)
        {
            if(args.Teamcity)return new TeamcityUpdater(updater, totalCount);
            return args.Labels ?
                new NUnitLabelsUpdater(updater, totalCount) :
                new GroupUpdater(updater, totalCount);
        }

        private static void PrintTotalInfo(ConsoleView view, string xmlReport, string outputReport, string path, string dir)
        {
            Directory.SetCurrentDirectory(dir);
            if (!string.IsNullOrEmpty(xmlReport)) view.GenerateReport(path, xmlReport);
            if (!string.IsNullOrEmpty(outputReport))
            {
                var totalResult = view.TotalResult;
                File.WriteAllText(
                    Path.Combine(Environment.CurrentDirectory, outputReport),
                    string.Join(string.Empty, totalResult.Metrics.All.Select(x => x.Output)));
            }
        }
    }
}
