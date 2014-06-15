using System;
using System.IO;
using System.Linq;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal class TestsExecutor : ITestsExecutor
    {
        private readonly IConsoleView view;
        private readonly IUpdater updater;

        public TestsExecutor(IConsoleView view, IUpdater updater)
        {
            this.view = view;
            this.updater = updater;
        }

        public int Run(CommandLineArgs args)
        {
            var workingDirectory = Environment.CurrentDirectory;
            using (var fixture = new MultiTestsFixture(
                args.Paths.Select(args.ToTestsConfig).ToArray(),
                args.AssembliesInParallel))
            {
                if (args.Logo) Console.WriteLine("Calculating tests.");
                var contexts = fixture.Refresh();
                var retValue = contexts.Min(x => x.RetValue);
                if (retValue == 0)
                {
                    var totalResults = new TestsResults(contexts.SelectMany(x => x.Results.Items).ToArray());
                    if (args.Logo)
                    {
                        Console.WriteLine("{0} tests found.", totalResults.Metrics.Total);
                        Console.WriteLine("Running tests.");
                    }

                    fixture.Run(
                        BuildUpdater(args, updater, totalResults.Metrics.Total), 
                        c => OnTestEnd(c, args));

                    view.PrintTotalResults();
                    PrintTotalInfo(view, args.XmlReport, args.OutputReport, args.Paths.FirstOrDefault(), workingDirectory);
                    return args.ReturnSuccess ? 0 : contexts.Min(x => x.RetValue);
                }
                return retValue;
            }
        }

        private void OnTestEnd(ExecutionContext context, CommandLineArgs args)
        {
            view.SetItems(context.Results);
            if (args.Logo && !args.Labels && args.Paths.Count > 1)
            {
                Console.WriteLine("'{0}' is done, time: {1}", Path.GetFileName(context.Path), ((Environment.TickCount - context.StartTime) / 1000).FormatTimeInSec());
            }
        }

        private static ITestsUpdater BuildUpdater(CommandLineArgs args, IUpdater updater, int totalCount)
        {
            if(args.Teamcity)return new TeamcityUpdater(updater, totalCount);
            return args.Labels ?
                new NUnitLabelsUpdater(updater, totalCount) :
                new GroupUpdater(updater, totalCount);
        }

        private static void PrintTotalInfo(IConsoleView view, string xmlReport, string outputReport, string path, string dir)
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
