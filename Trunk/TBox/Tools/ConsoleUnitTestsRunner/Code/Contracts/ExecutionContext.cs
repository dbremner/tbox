using LightInject;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts
{
    class ExecutionContext
    {
        public string Path { get; set; }
        public int RetValue { get; set; }
        public IThreadTestConfig Config { get; set; }
        public TestsResults Results { get; set; }
        public IPackage<IThreadTestConfig> Package { get; set; }
        public IServiceContainer Container { get; set; }

        public ExecutionContext()
        {
            RetValue = 0;
        }
    }
}
