using System;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IThreadTestConfig  : ITestsConfig
    {
        ResolveEventHandler ResolveEventHandler { get; }
    }
}
