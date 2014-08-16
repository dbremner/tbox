using System;
using System.Collections.Generic;

namespace Mnk.Library.ParallelNUnit.Core
{
    public class TestRunConfig
    {
        public IList<IList<int>> TestsToRun { get; private set; }
        public IList<string>DllPaths { get; private set; }
        public int StartDelay { get; set; }

        public TestRunConfig(IList<string> dllPaths)
        {
            DllPaths = dllPaths;
            TestsToRun = new List<IList<int>>();
        }
    }
}
