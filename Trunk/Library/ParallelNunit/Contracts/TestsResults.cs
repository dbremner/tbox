using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Packages.Common;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public class TestsResults
    {
        public Exception Exception { get; private set; }
        public bool IsFailed { get { return Exception != null; } }
        public ITestsMetricsCalculator Metrics { get; private set; }
        public IList<Result> Items { get; private set; }
       
        public TestsResults(IList<Result> items, Exception ex = null)
        {
            Metrics = new TestsMetricsCalculator();
            Metrics.Refresh(items);
            Items = items;
            Exception = ex;
        }

        public TestsResults(Exception ex=null):this(new Result[0], ex)
        {
        }
    }
}
