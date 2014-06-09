using System;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts
{
    class ExecutionContext
    {
        public int RetValue { get; set; }
        public IThreadTestConfig Config { get; set; }
        public TestsResults Results { get; set; }

        public ExecutionContext()
        {
            RetValue = 0;
        }
    }
}
