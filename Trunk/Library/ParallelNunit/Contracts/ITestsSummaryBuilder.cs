using System;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsSummaryBuilder
    {
        string Build(ITestsMetricsCalculator metrics, DateTimeOffset startTime);
    }
}
