using System;

namespace ParallelNUnit.Infrastructure.Runners
{
    public interface IContext : IDisposable
    {
        void WaitForExit();
        void Kill();
    }
}
