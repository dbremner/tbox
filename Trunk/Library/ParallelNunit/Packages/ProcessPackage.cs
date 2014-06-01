using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages
{
    sealed class ProcessPackage : BasePackage<IProcessTestConfig>
    {
        private readonly IProcessTestsRunner testsRunner;
        private readonly IProcessCalculator calculator;

        public ProcessPackage(IProcessTestConfig config, ITestsMetricsCalculator tmc, IPrefetchManager prefetchManager, ITestsView view, IProcessCalculator calculator, IProcessTestsRunner testsRunner)
            : base(config, tmc, prefetchManager, view)
        {
            this.calculator = calculator;
            this.testsRunner = testsRunner;
        }

        protected override void DoRefresh()
        {
            var client = ((NunitRunnerClient) Server.Owner);
            client.PrepareToCalc();
            calculator.CollectTests(Config.TestDllPath, Server.Handle);
            Items = client.Collection;
        }

        protected override void DoRun(IList<IList<Result>> packages)
        {
            testsRunner.Run(Items, packages, Server);
        }
    }
}
