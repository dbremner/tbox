using System;
using System.Collections.Generic;
using NUnit.Core;

namespace Mnk.Library.ParallelNUnit.Core
{
    [Serializable]
    public class Filter : ITestFilter
    {
        public ISet<int> Items { get; set; }
        public bool Pass(ITest test)
        {
            if (RemoteListener.ShouldStop) return false;
            return !string.Equals(test.TestType, "TestMethod") ||
                   Items.Contains(int.Parse(test.TestName.TestID.ToString()));
        }

        public bool Match(ITest test)
        {
            return true;
        }

        public bool IsEmpty { get { return false; } }
    }
}
