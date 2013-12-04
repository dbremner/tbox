using System.Collections.Generic;
using ParallelNUnit.Core;

namespace ParallelNUnit.Infrastructure.Interfaces
{
    public interface IUnitTestsView
    {
        void SetItems(IList<Result> items, TestsMetricsCalculator metrics);
        void Clear();
    }
}
