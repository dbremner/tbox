using System;
using System.Threading;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    public sealed class ThreadRunnerContext : IRunnerContext
    {
        private readonly Thread thread;

        public ThreadRunnerContext(Thread thread)
        {
            this.thread = thread;
        }

        public void Dispose()
        {
        }

        public void WaitForExit()
        {
            thread.Join();
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }
    }
}
