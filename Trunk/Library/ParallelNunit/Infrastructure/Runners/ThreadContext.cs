using System;
using System.Threading;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    public sealed class ThreadContext : IContext
    {
        private readonly Thread t;

        public ThreadContext(Thread t)
        {
            this.t = t;
        }

        public void Dispose()
        {
        }

        public void WaitForExit()
        {
            t.Join();
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }
    }
}
