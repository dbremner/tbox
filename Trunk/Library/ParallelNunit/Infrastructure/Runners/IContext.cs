using System;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    public interface IContext : IDisposable
    {
        void WaitForExit();
        void Kill();
    }
}
