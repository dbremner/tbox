using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages
{
    sealed class ThreadPackage : BasePackage<IThreadTestConfig>
    {
        private readonly IThreadTestsExecutor testsExecutor;
        private readonly IThreadTestsRunner testsRunner;

        public ThreadPackage(IOrderOptimizationManager orderOptimizationManager, IThreadTestsExecutor testsExecutor, IThreadTestsRunner testsRunner)
            : base(orderOptimizationManager)
        {
            this.testsExecutor = testsExecutor;
            this.testsRunner = testsRunner;
        }

        protected override TestsResults DoRefresh(IThreadTestConfig config)
        {
            var results = testsExecutor.CollectTests(config);
            if(results == null)
                throw new ArgumentException("Can't collect tests in: " + config.TestDllPath);
            return new TestsResults(new[]{results});
        }

        protected override TestsResults DoRun(IThreadTestConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, ITestsUpdater updater)
        {
            return testsRunner.Run(config, metrics, allTests, packages, Server, updater);
        }
    }
}
