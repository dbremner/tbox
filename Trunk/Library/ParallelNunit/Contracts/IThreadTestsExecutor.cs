using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IThreadTestsExecutor
    {
        Result CollectTests();
        int RunTests(string handle);
    }
}