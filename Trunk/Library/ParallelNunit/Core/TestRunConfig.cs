﻿using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Core
{
    public class TestRunConfig
    {
        public IList<IList<int>> TestsToRun { get; set; }
        public IList<string>DllPaths { get; private set; }
        public int StartDelay { get; set; }

        public TestRunConfig(IList<string> dllPaths)
        {
            DllPaths = dllPaths;
            TestsToRun = new List<IList<int>>();
        }
    }
}