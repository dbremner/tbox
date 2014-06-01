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
        private readonly ITestsView view;
        protected InterprocessServer<INunitRunnerClient> Server { get; private set; }
        private readonly ILog log = LogManager.GetLogger<BasePackage<TConfig>>();
        private IList<Result> collected;

        public TConfig Config { get; private set; }
        public ITestsMetricsCalculator Metrics { get; private set; }
        public IList<Result> Items
        {
            get { return collected; }
            set
            {
                collected = value;
                Metrics.Refresh(value);
            }
        }
        public event Action<IPackage<TConfig>> RefreshSuccessEventHandler;
        public event Action<IPackage<TConfig>> RefreshErrorEventHandler;
        public event Action<IPackage<TConfig>> TestsFinishedEventHandler;

        protected BasePackage(TConfig config, ITestsMetricsCalculator metrics, IOrderOptimizationManager orderOptimizationManager, ITestsView view)
        {
            this.orderOptimizationManager = orderOptimizationManager;
            this.view = view;
            Config = config;
            Metrics = metrics;
            Server = new InterprocessServer<INunitRunnerClient>(new NunitRunnerClient());
            Items = new Result[0];
        }

        public bool EnsurePathIsValid()
        {
            if (File.Exists(Config.TestDllPath)) return true;
            log.Write("Can't load dll: " + Config.TestDllPath);
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

        public IList<IList<Result>> DivideTests(IList<Result> checkedTests=null)
        {
            ResetTests(checkedTests);
            var items = checkedTests ?? Metrics.Tests;
            var filter = GetFilter(Config.Categories, Config.IncludeCategories);
            return (Config.ProcessCount > 1)
                                ? DivideTestsToRun(items.Where(filter).ToArray(), Config.ProcessCount, Config.OptimizeOrder).ToArray()
                                : new IList<Result>[] { items.Where(filter).ToArray() };
        }

        private void ResetTests(IList<Result> checkedTests)
        {
            var items = checkedTests ?? Metrics.All;
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

        private IEnumerable<IList<Result>> DivideTestsToRun(IList<Result> tests, int threadCount, bool optimizeOrder)
        {
            if (optimizeOrder && threadCount > 1)
            {
                tests = orderOptimizationManager.Optimize(Config.TestDllPath, tests);
            }
            var result = new List<IList<Result>>();
            for (var j = 0; j < threadCount; ++j)
            {
                result.Add(new List<Result>(tests.Count / threadCount));
            }
            for (var i = 0; i < tests.Count; )
            {
                for (var j = 0; j < threadCount && i < tests.Count; ++j)
                {
                    result[j].Add(tests[i++]);
                }
            }
            return result;
        }

        public void Refresh()
        {
            try
            {
                DoRefresh();
                if (RefreshSuccessEventHandler!=null) RefreshSuccessEventHandler(this);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't refresh tests from dll: " + Config.TestDllPath);
                if (RefreshErrorEventHandler!=null) RefreshErrorEventHandler(this);
            }
        }

        public void Run(IList<Result> checkedTests = null)
        {
            try
            {
                DoRun(DivideTests(checkedTests));
                ApplyResults();
                if (TestsFinishedEventHandler!=null) TestsFinishedEventHandler(this);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't run test, from dll: " + Config.TestDllPath);
            }
        }


        protected abstract void DoRefresh();
        protected abstract void DoRun(IList<IList<Result>> packages);

        private void ApplyResults()
        {
            Metrics.Refresh(Items);
            view.SetItems(Items, Metrics);
            if (Config.OptimizeOrder)
            {
                orderOptimizationManager.SaveStatistic(Config.TestDllPath, Metrics.Tests);
            }
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
