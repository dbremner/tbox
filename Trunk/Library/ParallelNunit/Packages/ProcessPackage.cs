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

        public ProcessPackage(IOrderOptimizationManager orderOptimizationManager, IProcessCalculator calculator, IProcessTestsRunner testsRunner)
            : base(orderOptimizationManager)
        {
            this.calculator = calculator;
            this.testsRunner = testsRunner;
        }

        protected override TestsResults DoRefresh(IProcessTestConfig config)
        {
            var client = ((NunitRunnerClient) Server.Owner);
            client.PrepareToCalc();
            calculator.CollectTests(config, config.TestDllPath, Server.Handle);
            return new TestsResults(client.Collection);
        }

        protected override TestsResults DoRun(IProcessTestConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, ITestsUpdater updater)
        {
            return testsRunner.Run(config, metrics, allTests, packages, Server, updater);
        }
    }
}
