using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Interfaces
{
    public interface IUnitTestsView
    {
        void SetItems(IList<Result> items, TestsMetricsCalculator metrics);
        void Clear();
    }
}
