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

        public ThreadPackage(IThreadTestConfig config, ITestsMetricsCalculator metrics, IOrderOptimizationManager orderOptimizationManager, ITestsView view, IThreadTestsExecutor testsExecutor, IThreadTestsRunner testsRunner)
            : base(config, metrics, orderOptimizationManager, view)
        {
            this.testsExecutor = testsExecutor;
            this.testsRunner = testsRunner;
        }

        protected override void DoRefresh()
        {
            var results = testsExecutor.CollectTests();
            if(results == null)
                throw new ArgumentException("Can't collect tests in: " + Config.TestDllPath);
            Items = new[]{results};
        }

        protected override void DoRun(IList<IList<Result>> packages)
        {
            testsRunner.Run(Items, packages, Server);
        }
    }
}
