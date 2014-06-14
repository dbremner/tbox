using System;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class InternalTestsExecutionFacade : ITestsExecutionFacade
    {
        private readonly INUnitTestFacade executor;

        public InternalTestsExecutionFacade(INUnitTestFacade executor)
        {
            this.executor = executor;
        }

        public TestsResults CollectTests(ITestsConfig config, InterprocessServer<INunitRunnerClient> server)
        {
            var results = executor.CollectTests(config.TestDllPath, config.RuntimeFramework);
            if (results == null)
                throw new ArgumentException("Can't collect tests in: " + config.TestDllPath);
            return new TestsResults(new[] { results });
        }

        public void Run(ITestsConfig config, string handle)
        {
            executor.RunTests(config, handle);
        }
    }
}
