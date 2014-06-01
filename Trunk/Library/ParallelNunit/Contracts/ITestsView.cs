using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsView
    {
        void SetItems(IList<Result> items, ITestsMetricsCalculator metrics);
        void Clear();
    }
}
