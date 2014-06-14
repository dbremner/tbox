using System;
using System.Threading;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ThreadTestsExecutionFacade : ITestsExecutionFacade
    {
        private readonly INUnitTestFacade executor;

        public ThreadTestsExecutionFacade(INUnitTestFacade executor)
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

        public IRunnerContext Run(ITestsConfig config, string handle)
        {
            var t = new Thread(o => executor.RunTests(config, handle));
            t.Start();
            return new ThreadRunnerContext(t);
        }
    }
}
