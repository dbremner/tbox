using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface INUnitTestFacade
    {
        int RunTests(ITestsConfig config, string handle);
        int Run(string handle, string path, int[] items, bool fast, bool needOutput, string runtimeFramework);
        Result CollectTests(string path, string runtimeFramework);
    }
}