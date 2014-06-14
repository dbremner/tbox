using System;
using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages
{
    class TestsFixture: ITestsFixture
    {
        private readonly IOrderOptimizationManager orderOptimizationManager;
        protected InterprocessServer<INunitRunnerClient> Server { get; private set; }
        private readonly ILog log = LogManager.GetLogger<TestsFixture>();
        private readonly ITestsExecutor testsExecutor;
        private readonly ITestsDivider testsDivider;

        public TestsFixture(IOrderOptimizationManager orderOptimizationManager, ITestsExecutor testsExecutor, ITestsDivider testsDivider)
        {
            this.orderOptimizationManager = orderOptimizationManager;
            this.testsExecutor = testsExecutor;
            this.testsDivider = testsDivider;
            Server = new InterprocessServer<INunitRunnerClient>(new NunitRunnerClient());
        }

        public bool EnsurePathIsValid(ITestsConfig config)
        {
            if (File.Exists(config.TestDllPath)) return true;
            log.Write("Can't load dll: " + config.TestDllPath);
            return false;
        }

        public TestsResults Refresh(ITestsConfig config)
        {
            try
            {
                var client = ((NunitRunnerClient)Server.Owner);
                client.PrepareToCalc();
                return testsExecutor.CollectTests(config, Server);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't refresh tests from dll: " + config.TestDllPath);
                return new TestsResults(ex);
            }
        }

        public TestsResults Run(ITestsConfig config, TestsResults tests, ITestsUpdater updater, IList<Result> checkedTests = null)
        {
            try
            {
                var divided = testsDivider.Divide(config, tests.Metrics, checkedTests);
                var r = testsExecutor.Run(config, tests.Metrics, tests.Items, divided, Server, updater);
                if (config.OptimizeOrder)
                {
                    orderOptimizationManager.SaveStatistic(config.TestDllPath, r.Metrics.Tests);
                }
                return r;
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't run test, from dll: " + config.TestDllPath);
                return new TestsResults();
            }
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
