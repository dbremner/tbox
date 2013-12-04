using System;
using System.Threading;
using ParallelNUnit.Execution;

namespace ParallelNUnit.Infrastructure.Runners
{
    class ThreadTestsRunner : TestsRunner
    {
        private readonly ResolveEventHandler loadFromSameFolder;
        public ThreadTestsRunner(ResolveEventHandler loadFromSameFolder)
        {
            this.loadFromSameFolder = loadFromSameFolder;
        }

        protected override IContext Run(bool needSynchronizationForTests, string handle, bool needOutput)
        {
            var t = new Thread(o => new NUnitExecutor(loadFromSameFolder).RunTests(handle, !needSynchronizationForTests, needOutput));
            t.Start();
            return new ThreadContext(t);
        }

    }
}
