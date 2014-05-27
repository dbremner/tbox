using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    using System.Globalization;
    using System.Linq;

    internal class ConsoleView : IUnitTestsView
    {
        private List<TestResults> Results { get; set; }

        private DateTime CurrentAssemblyStartTime { get; set; }
        
        public ConsoleView()
        {
            this.Results = new List<TestResults>();
        }

        public void NotifyNewAssemblyStartTest()
        {
            this.CurrentAssemblyStartTime = DateTime.Now;
        }

        public void SetItems(IList<Result> items, TestsMetricsCalculator metrics)
        {
            var elapsedTime = DateTime.Now - this.CurrentAssemblyStartTime;
            var currentAssemblyResults = new TestResults(items, metrics, elapsedTime);

            this.Results.Add(currentAssemblyResults);

            Console.WriteLine();
            Console.WriteLine(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds\n  Not run: {5}, Invalid: {6}, Ignored: {7}, Skipped: {8}",
                currentAssemblyResults.Metrics.Passed,
                currentAssemblyResults.Metrics.Errors,
                currentAssemblyResults.Metrics.Failures,
                currentAssemblyResults.Metrics.Inconclusive,
                currentAssemblyResults.ElapsedTime.TotalSeconds.ToString("F2", CultureInfo.InvariantCulture),
                currentAssemblyResults.Metrics.NotRun.Length,
                currentAssemblyResults.Metrics.Invalid,
                currentAssemblyResults.Metrics.Ignored,
                currentAssemblyResults.Metrics.Skipped
                );

            PrintArray("Errors and Failures:", currentAssemblyResults.Metrics.Failed, true);
            PrintArray("Tests Not Run:", currentAssemblyResults.Metrics.NotRun, false);

            Console.WriteLine();
        }

        private static void PrintArray(string message, Result[] items, bool stackTrace)
        {
            if (items.Length == 0) return;
            Console.WriteLine();
            Console.WriteLine(message);
            var i = 0;
            foreach (var r in items)
            {
                Console.WriteLine("{0}) {1} : {2}{3}{4}", ++i, r.State, r.FullName,
                string.IsNullOrEmpty(r.Message) ? string.Empty : ("\n   " + r.Message),
                stackTrace ? ("\n" + r.StackTrace + "\n") : string.Empty
                );
            }
            Console.WriteLine();
        }

        public void Clear()
        {
        }
        
        public void PrintTotalResults()
        {
            if (this.Results.Count < 2)
            {
                return;
            }

            var totalResult = this.CreateTotalResult();

            Console.WriteLine();
            Console.WriteLine("Sum of all tests:");
            Console.WriteLine(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds\n  Not run: {5}, Invalid: {6}, Ignored: {7}, Skipped: {8}",
                totalResult.Metrics.Passed,
                totalResult.Metrics.Errors,
                totalResult.Metrics.Failures,
                totalResult.Metrics.Inconclusive,
                totalResult.ElapsedTime.TotalSeconds.ToString("F2", CultureInfo.InvariantCulture),
                totalResult.Metrics.NotRun.Length,
                totalResult.Metrics.Invalid,
                totalResult.Metrics.Ignored,
                totalResult.Metrics.Skipped
                );

            PrintArray("Errors and Failures:", totalResult.Metrics.Failed, true);
            PrintArray("Tests Not Run:", totalResult.Metrics.NotRun, false);

            Console.WriteLine();
        }

        public TestResults CreateTotalResult()
        {
            var totalResults = this.Results.SelectMany(results => results.Result).ToList();
            var totalMetrics = new TestsMetricsCalculator(totalResults);
            var elapsedTime = this.Results.Aggregate(TimeSpan.Zero, (current, result) => current + result.ElapsedTime);

            return new TestResults(totalResults, totalMetrics, elapsedTime);
        }

        public void GenerateReport(string path, string xmlReport)
        {
            if (this.Results.Count < 1)
            {
                return;
            }

            var totalResult = this.CreateTotalResult();
            ReportBuilder.GenerateReport(path, xmlReport, totalResult.Metrics, totalResult.Result);
        }

        internal class TestResults
        {
            public IList<Result> Result { get; private set; }

            public TestsMetricsCalculator Metrics { get; private set; }

            public TimeSpan ElapsedTime { get; private set; }

            public TestResults(IList<Result> result, TestsMetricsCalculator metrics, TimeSpan elapsedTime)
            {
                this.Result = result;
                this.Metrics = metrics;
                this.ElapsedTime = elapsedTime;
            }
        }
    }
}
