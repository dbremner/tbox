using System;
using System.Threading;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    public sealed class ThreadContext : IContext
    {
        private readonly Thread thread;

        public ThreadContext(Thread thread)
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
