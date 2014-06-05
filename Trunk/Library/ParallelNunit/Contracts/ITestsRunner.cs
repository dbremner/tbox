using System.Collections.Generic;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsRunner<in TConfig>
    {
        TestsResults CollectTests(TConfig config, InterprocessServer<INunitRunnerClient> server);
        TestsResults Run(TConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, ITestsUpdater updater);
    }
}