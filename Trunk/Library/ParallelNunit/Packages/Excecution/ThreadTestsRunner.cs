using System.Threading;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages.Common;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ThreadTestsRunner : TestsRunner, IThreadTestsRunner
    {
        private readonly IThreadTestsExecutor testsExecutor;

        public ThreadTestsRunner(IThreadTestsExecutor testsExecutor, IDirectoriesManipulator directoriesManipulator, ITestsConfig config, ITestsUpdater updater, ISynchronizer synchronizer, ITestsMetricsCalculator calculator)
            :base(directoriesManipulator, config, updater, synchronizer, calculator)
        {
            this.testsExecutor = testsExecutor;
        }

        protected override IRunnerContext Run(string handle)
        {
            var t = new Thread(o => testsExecutor.RunTests(handle));
            t.Start();
            return new ThreadRunnerContext(t);
        }
    }
}
