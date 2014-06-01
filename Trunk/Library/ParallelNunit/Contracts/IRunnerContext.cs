using System;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IRunnerContext : IDisposable
    {
        void WaitForExit();
        void Kill();
    }
}
