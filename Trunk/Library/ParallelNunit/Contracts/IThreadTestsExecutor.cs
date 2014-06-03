using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IThreadTestsExecutor
    {
        Result CollectTests(IThreadTestConfig config);
        int RunTests(IThreadTestConfig config, string handle);
    }
}