using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly static ILog Log = LogManager.GetLogger<TestsExecutor>();

        public TestsExecutor(IReportBuilder reportBuilder, IPackage<IThreadTestConfig> package, IUpdater updater)
        {
            this.reportBuilder = reportBuilder;
            this.package = package;
            this.updater = updater;
        }

        public int Run(CommandLineArgs args)
        {
            var workingDirectory = Environment.CurrentDirectory;
            var view = new ConsoleView(reportBuilder);

            if (args.Logo) Console.WriteLine("Calculating tests.");
            var assemblies = CollectTests(args.Paths, args);
            if (assemblies.All(x => x.Value.RetValue == 0))
            {
                var totalResults = new TestsResults(assemblies.SelectMany(x => x.Value.Results.Items).ToArray());
                if (args.Logo)
                {
                    Console.WriteLine("{0} tests found.", totalResults.Metrics.Total);
                    Console.WriteLine("Running tests.");
                }
                var testsUpdater = BuildUpdater(args, updater, totalResults.Metrics.Total);

                if (args.AssembliesInParallel > 1)
                {
                    Parallel.ForEach(assemblies, 
                        new ParallelOptions{MaxDegreeOfParallelism = args.AssembliesInParallel},
                        assembly =>
                        {
                            using (var c = Library.ParallelNUnit.ServicesRegistrar.Register())
                            {
                                RunTest(assembly.Value, view, testsUpdater, c.GetInstance<IPackage<IThreadTestConfig>>());
                            }
                        });
                }
                else
                {
                    foreach (var assembly in assemblies)
                    {
                        RunTest(assembly.Value, view, testsUpdater,package);
                    }
                }

                view.PrintTotalResults();
                PrintTotalInfo(view, args.XmlReport, args.OutputReport, args.Paths.FirstOrDefault(), workingDirectory);
            }
            return assemblies.Min(x=>x.Value.RetValue);
        }

        private IDictionary<string, ExecutionContext> CollectTests(IList<string> paths, CommandLineArgs args)
        {
            return paths.Count == 1 ? 
                new Dictionary<string, ExecutionContext> { { paths[0], Collect(paths[0], args, package) } } : 
                paths.AsParallel().ToDictionary(x => x, x =>
                {
                    using (var c = Library.ParallelNUnit.ServicesRegistrar.Register())
                    {
                        return Collect(x, args, c.GetInstance<IPackage<IThreadTestConfig>>());
                    }
                });
        }

        private static ExecutionContext Collect(string path, CommandLineArgs args, IPackage<IThreadTestConfig> package)
        {
            var context = new ExecutionContext
            {
                Config = CreateConfig(args, path),
                RetValue = 0
            };
            if (!package.EnsurePathIsValid(context.Config))
            {
                Log.Write("Incorrect path: " + path);
                context.RetValue = -3;
            }
            else
            {
                context.Results = package.Refresh(context.Config);
                if (context.Results.IsFailed)
                {
                    Log.Write("Can't calculate tests count");
                    context.RetValue = -3;
                }
            }
            return context;
        }

        private static void RunTest(ExecutionContext context, ConsoleView view, ITestsUpdater testsUpdater, IPackage<IThreadTestConfig> package )
        {
            context.Results = package.Run(context.Config, context.Results, testsUpdater);
            view.SetItems(context.Results);
            if (context.Results.Metrics.FailedCount > 0)
            {
                context.RetValue = -2;
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
