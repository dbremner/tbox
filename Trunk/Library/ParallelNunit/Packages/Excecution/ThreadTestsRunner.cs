using System;
using System.Threading;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Packages.Common;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ThreadTestsRunner : TestsRunner<IThreadTestConfig>
    {
        private readonly IThreadTestsExecutor executor;

        public ThreadTestsRunner(IDirectoriesManipulator directoriesManipulator, IThreadTestsExecutor executor)
            :base(directoriesManipulator)
        {
            this.executor = executor;
        }

        public override TestsResults CollectTests(IThreadTestConfig config, InterprocessServer<INunitRunnerClient> server)
        {
            var results = new NUnitTestStarter().CollectTests(config.TestDllPath, config.RuntimeFramework);
            if (results == null)
                throw new ArgumentException("Can't collect tests in: " + config.TestDllPath);
            return new TestsResults(new[] { results });
        }

        protected override IRunnerContext DoRun(IThreadTestConfig config, string handle)
        {
            var t = new Thread(o => executor.RunTests(config, handle));
            t.Start();
            return new ThreadRunnerContext(t);
        }
    }
}
