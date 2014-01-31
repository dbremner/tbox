using System.Collections.Generic;
using System.Linq;
using Mnk.Library.ParallelNUnit.Core;
using NUnit.Core;

namespace Mnk.Library.ParallelNUnit.Infrastructure
{
    public class TestsMetricsCalculator
    {
        public int Errors { get; private set; }
        public int Failures { get; private set; }
        public int Skipped { get; private set; }
        public int Invalid { get; private set; }
        public int Inconclusive { get; private set; }
        public int Ignored { get; private set; }
        public int Passed { get; private set; }
        public int Total { get; private set; }
        public Result[] Failed { get; set; }
        public Result[] NotRun { get; set; }
        public IList<Result> Tests { get; private set; }
        public IList<Result> All { get; private set; }

        public TestsMetricsCalculator(IEnumerable<Result> items)
        {
            All = items.SelectMany(Collect).ToArray();
            Tests = All.Where(x=>x.IsTest).ToArray();

            Failed = Tests.Where(x => x.State == ResultState.Error || x.State == ResultState.Failure).ToArray();
            NotRun = Tests.Where(x => x.State == ResultState.Ignored || x.State == ResultState.Skipped).ToArray();

            Errors = Failed.Count(x => x.State == ResultState.Error);
            Failures = Failed.Count(x => x.State == ResultState.Failure);

            Ignored = NotRun.Count(x => x.State == ResultState.Ignored);
            Skipped = NotRun.Count(x => x.State == ResultState.Skipped);

            Invalid = Tests.Count(x => x.State == ResultState.NotRunnable);
            Inconclusive = Tests.Count(x => x.State == ResultState.Inconclusive);

            Total = Tests.Count;
            Passed = Total - Ignored - Inconclusive - Skipped - Invalid - Failures - Errors;
        }

        public static IEnumerable<Result> Collect(Result result)
        {
            foreach (var r in result.Children.Cast<Result>().SelectMany(Collect))
            {
                yield return r;
            }
            yield return result;
        }
    }

}
