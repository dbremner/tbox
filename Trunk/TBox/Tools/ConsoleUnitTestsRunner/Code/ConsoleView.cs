using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal class ConsoleView : IConsoleView
    {
        private readonly DateTimeOffset startTime = DateTime.UtcNow;
        private readonly IReportBuilder reportBuilder;
        private readonly ITestsSummaryBuilder testsSummaryBuilder;
        public TestsResults TotalResult { get; private set; }
        private readonly List<TestsResults> results;
        private readonly object sync = new object();

        public ConsoleView(IReportBuilder reportBuilder, ITestsSummaryBuilder testsSummaryBuilder)
        {
            this.reportBuilder = reportBuilder;
            this.testsSummaryBuilder = testsSummaryBuilder;
            results = new List<TestsResults>();
        }

        public void SetItems(TestsResults result)
        {
            lock (sync)
            {
                results.Add(result);
            }
        }

        public void PrintTotalResults()
        {
            TotalResult = new TestsResults(results.SelectMany(x => x.Items).ToArray());
            Console.WriteLine(testsSummaryBuilder.Build(TotalResult.Metrics, startTime)); 
        }

        public void GenerateReport(string path, string xmlReport)
        {
            reportBuilder.GenerateReport(path, xmlReport, TotalResult.Metrics, TotalResult.Items);
        }

    }
}
