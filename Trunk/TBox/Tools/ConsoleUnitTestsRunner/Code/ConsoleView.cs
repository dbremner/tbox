using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal class ConsoleView
    {
        private readonly DateTimeOffset startTime = DateTime.UtcNow;
        private readonly IReportBuilder reportBuilder;
        public TestsResults TotalResult { get; private set; }
        private readonly List<TestsResults> results;

        public ConsoleView(IReportBuilder reportBuilder)
        {
            this.reportBuilder = reportBuilder;
            results = new List<TestsResults>();
        }

        public void SetItems(TestsResults result)
        {
            results.Add(result);
        }

        public void PrintTotalResults()
        {
            TotalResult = new TestsResults(results.SelectMany(x => x.Items).ToArray());
            Console.WriteLine(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds\n  Not run: {5}, Invalid: {6}, Ignored: {7}, Skipped: {8}",
                TotalResult.Metrics.Passed,
                TotalResult.Metrics.Errors,
                TotalResult.Metrics.Failures,
                TotalResult.Metrics.Inconclusive,
                (DateTime.UtcNow - startTime).TotalSeconds.ToString("F2", CultureInfo.InvariantCulture),
                TotalResult.Metrics.NotRun.Length,
                TotalResult.Metrics.Invalid,
                TotalResult.Metrics.Ignored,
                TotalResult.Metrics.Skipped
                );

            PrintArray("Errors and Failures:", TotalResult.Metrics.Failed, true);
            PrintArray("Tests Not Run:", TotalResult.Metrics.NotRun, false);

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

        public void GenerateReport(string path, string xmlReport)
        {
            reportBuilder.GenerateReport(path, xmlReport, TotalResult.Metrics, TotalResult.Items);
        }

    }
}
