using System.Threading;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages.Common;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ThreadTestsRunner : TestsRunner<IThreadTestConfig>, IThreadTestsRunner
    {
        private readonly IThreadTestsExecutor testsExecutor;

        public ThreadTestsRunner(IThreadTestsExecutor testsExecutor, IDirectoriesManipulator directoriesManipulator)
            :base(directoriesManipulator)
        {
            this.testsExecutor = testsExecutor;
        }

        protected override IRunnerContext DoRun(IThreadTestConfig config, string handle)
        {
            var t = new Thread(o => testsExecutor.RunTests(config, handle));
            t.Start();
            return new ThreadRunnerContext(t);
        }
    }
}
