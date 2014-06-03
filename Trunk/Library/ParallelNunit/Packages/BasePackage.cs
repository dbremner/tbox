using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using NUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages
{
    public abstract class BasePackage<TConfig> : IPackage<TConfig>
        where TConfig: ITestsConfig
    {
        private readonly IOrderOptimizationManager orderOptimizationManager;
        protected InterprocessServer<INunitRunnerClient> Server { get; private set; }
        private readonly ILog log = LogManager.GetLogger<BasePackage<TConfig>>();

        protected BasePackage(IOrderOptimizationManager orderOptimizationManager)
        {
            this.orderOptimizationManager = orderOptimizationManager;
            Server = new InterprocessServer<INunitRunnerClient>(new NunitRunnerClient());
        }

        public bool EnsurePathIsValid(TConfig config)
        {
            if (File.Exists(config.TestDllPath)) return true;
            log.Write("Can't load dll: " + config.TestDllPath);
            return false;
        }

        private static bool IncludeFilter(Result r, IEnumerable<string> values)
        {
            return r.Categories.Any(o => values.Any(x => x.EqualsIgnoreCase(o)));
        }

        private static bool ExcludeFilter(Result r, IEnumerable<string> values)
        {
            return r.Categories.All(o => !values.Any(x => x.EqualsIgnoreCase(o)));
        }

        public IList<IList<Result>> DivideTests(TConfig config, ITestsMetricsCalculator metrics, IList<Result> checkedTests = null)
        {
            ResetTests(metrics, checkedTests);
            var items = checkedTests ?? metrics.Tests;
            var filter = GetFilter(config.Categories, config.IncludeCategories);
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
                i.Message = i.StackTrace = i.Description = string.Empty;
                i.Time = i.AssertCount = 0;
                i.Executed = false;
                i.Output = string.Empty;
                i.Refresh();
            }
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

        private IEnumerable<IList<Result>> DivideTestsToRun(TConfig config, IList<Result> tests)
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

        public TestsResults Refresh(TConfig config)
        {
            try
            {
                return DoRefresh(config);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't refresh tests from dll: " + config.TestDllPath);
                return new TestsResults();
            }
        }

        public TestsResults Run(TConfig config, TestsResults tests, ITestsUpdater updater, IList<Result> checkedTests = null)
        {
            try
            {
                var r = DoRun(config, tests.Metrics, tests.Items, DivideTests(config, tests.Metrics, checkedTests), updater);
                if (config.OptimizeOrder)
                {
                    orderOptimizationManager.SaveStatistic(config.TestDllPath, r.Metrics.Tests);
                }
                return r;
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't run test, from dll: " + config.TestDllPath);
                return new TestsResults();
            }
        }

        protected abstract TestsResults DoRefresh(TConfig config);
        protected abstract TestsResults DoRun(TConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, ITestsUpdater updater);

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
