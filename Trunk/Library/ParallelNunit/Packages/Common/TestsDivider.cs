using System;
using System.Collections.Generic;
using System.Linq;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using NUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages.Common
{
    class TestsDivider : ITestsDivider
    {
        private readonly IOrderOptimizationManager orderOptimizationManager;

        public TestsDivider(IOrderOptimizationManager orderOptimizationManager)
        {
            this.orderOptimizationManager = orderOptimizationManager;
        }

        public IList<IList<Result>> Divide(ITestsConfig config, ITestsMetricsCalculator metrics, IList<Result> checkedTests)
        {
            ResetTests(metrics, checkedTests);
            var filter = GetFilter(config.Categories, config.IncludeCategories);
            var items = checkedTests ?? metrics.Tests;
            return (config.ProcessCount > 1)
                                ? DivideTestsToRun(config, items.Where(filter).ToArray()).ToArray()
                                : new IList<Result>[] { items.Where(filter).ToArray() };
        }

        private static void ResetTests(ITestsMetricsCalculator metrics, IList<Result> checkedTests)
        {
            var items = checkedTests ?? metrics.All;
            foreach (var i in items)
            {
                i.State = ResultState.NotRunnable;
                i.FailureSite = FailureSite.Test;
                i.Message = i.StackTrace = i.Description = string.Empty;
                i.Time = i.AssertCount = 0;
                i.Output = string.Empty;
                i.Refresh();
            }
        }

        private IEnumerable<IList<Result>> DivideTestsToRun(ITestsConfig config, IList<Result> tests)
        {
            if (config.OptimizeOrder && config.ProcessCount > 1)
            {
                tests = orderOptimizationManager.Optimize(config.TestDllPath, tests);
            }
            var result = new List<IList<Result>>();
            for (var j = 0; j < config.ProcessCount; ++j)
            {
                result.Add(new List<Result>(tests.Count / config.ProcessCount));
            }
            for (var i = 0; i < tests.Count; )
            {
                for (var j = 0; j < config.ProcessCount && i < tests.Count; ++j)
                {
                    result[j].Add(tests[i++]);
                }
            }
            return result;
        }

        private static bool IncludeFilter(Result r, IEnumerable<string> values)
        {
            return r.Categories.Any(o => values.Any(x => x.EqualsIgnoreCase(o)));
        }

        private static bool ExcludeFilter(Result r, IEnumerable<string> values)
        {
            return r.Categories.All(o => !values.Any(x => x.EqualsIgnoreCase(o)));
        }

        private static Func<Result, bool> GetFilter(string[] categories, bool? include)
        {
            if (categories == null || !include.HasValue || categories.Length <= 0) return r => true;
            if (include.Value)
            {
                return r => IncludeFilter(r, categories);
            }
            return r => ExcludeFilter(r, categories);
        }
    }
}
