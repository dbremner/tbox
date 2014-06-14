using System.Collections.Generic;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    internal interface ITestsExecutor
    {
        TestsResults CollectTests(ITestsConfig config, InterprocessServer<INunitRunnerClient> server);
        TestsResults Run(ITestsConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, ITestsUpdater updater);
    }
}