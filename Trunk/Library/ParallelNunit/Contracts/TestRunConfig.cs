using System.Collections.Generic;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public class TestRunConfig
    {
        private readonly ITestsConfig config;
        public IList<IList<int>> TestsToRun { get; set; }
        public IList<string>DllPaths { get; private set; }
        public int StartDelay { get; set; }
        public ITestsConfig Config { get; set; }

        public TestRunConfig(IList<string> dllPaths, ITestsConfig config)
        {
            this.config = config;
            DllPaths = dllPaths;
            TestsToRun = new List<IList<int>>();
        }
    }
}
