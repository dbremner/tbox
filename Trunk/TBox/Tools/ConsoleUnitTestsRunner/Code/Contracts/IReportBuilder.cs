using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts
{
    internal interface IReportBuilder
    {
        void GenerateReport(string path, string xmlReport, ITestsMetricsCalculator tmc, IEnumerable<Result> testResults );
    }
}