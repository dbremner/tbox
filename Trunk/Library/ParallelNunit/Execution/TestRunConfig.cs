using System.Collections.Generic;

namespace Mnk.Library.ParallelNUnit.Execution
{
    public class TestRunConfig
    {
        public IList<IList<int>> TestsToRun { get; set; }
        public IList<string>DllPathes { get; set; }
        public int StartDelay { get; set; }

        public TestRunConfig()
        {
            TestsToRun = new List<IList<int>>();
        }
    }
}
