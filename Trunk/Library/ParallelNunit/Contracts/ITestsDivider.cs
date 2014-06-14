using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsDivider
    {
        IList<IList<Result>> Divide(ITestsConfig config, ITestsMetricsCalculator metrics, IList<Result> checkedTests);
    }
}
