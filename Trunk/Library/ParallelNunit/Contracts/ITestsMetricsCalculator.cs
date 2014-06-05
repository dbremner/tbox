using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsMetricsCalculator
    {
        int Errors { get; }
        int Failures { get; }
        int Skipped { get; }
        int Invalid { get; }
        int Inconclusive { get; }
        int Ignored { get; }
        int Passed { get; }
        int Total { get; }
        Result[] Failed { get; set; }
        Result[] NotRun { get; set; }
        IList<Result> Tests { get; }
        IList<Result> All { get; }
        int FailedCount { get; }
    }
}