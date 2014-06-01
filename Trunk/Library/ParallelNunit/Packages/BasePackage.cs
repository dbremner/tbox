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
        private readonly IPrefetchManager prefetchManager;
        private readonly ITestsView view;
        protected InterprocessServer<INunitRunnerClient> Server { get; private set; }
        protected readonly ILog Log = LogManager.GetLogger<BasePackage<TConfig>>();
        private IList<Result> collected;

        public TConfig Config { get; private set; }
        public ITestsMetricsCalculator Tmc { get; private set; }
        public IList<Result> Items
        {
            get { return collected; }
            set
            {
                collected = value;
                Tmc.Refresh(value);
            }
        }
        public event Action<IPackage<TConfig>> RefreshSuccessEvent;
        public event Action<IPackage<TConfig>> RefreshErrorEvent;
        public event Action<IPackage<TConfig>> TestsFinishedEvent;

        protected BasePackage(TConfig config, ITestsMetricsCalculator tmc, IPrefetchManager prefetchManager, ITestsView view)
        {
            this.prefetchManager = prefetchManager;
            this.view = view;
            Config = config;
            Tmc = tmc;
            Server = new InterprocessServer<INunitRunnerClient>(new NunitRunnerClient());
            Items = new Result[0];
        }

        public bool EnsurePathIsValid()
        {
            if (File.Exists(Config.TestDllPath)) return true;
            Log.Write("Can't load dll: " + Config.TestDllPath);
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
            var items = checkedTests ?? Tmc.Tests;
            var filter = GetFilter(Config.Categories, Config.IncludeCategories);
            return (Config.ProcessCount > 1)
                                ? DivideTestsToRun(items.Where(filter).ToArray(), Config.ProcessCount, Config.UsePrefetch).ToArray()
                                : new IList<Result>[] { items.Where(filter).ToArray() };
        }

        private void ResetTests(IList<Result> checkedTests)
        {
            var items = checkedTests ?? Tmc.All;
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

        private IEnumerable<IList<Result>> DivideTestsToRun(IList<Result> tests, int threadCount, bool usePrefetch)
        {
            if (usePrefetch && threadCount > 1)
            {
                tests = prefetchManager.Optimize(Config.TestDllPath, tests);
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
                if (RefreshSuccessEvent!=null) RefreshSuccessEvent(this);
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Can't refresh tests from dll: " + Config.TestDllPath);
                if (RefreshErrorEvent!=null) RefreshErrorEvent(this);
            }
        }

        public void Run(IList<Result> checkedTests = null)
        {
            try
            {
                DoRun(DivideTests(checkedTests));
                ApplyResults();
                if (TestsFinishedEvent!=null) TestsFinishedEvent(this);
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Can't run test, from dll: " + Config.TestDllPath);
            }
        }


        protected abstract void DoRefresh();
        protected abstract void DoRun(IList<IList<Result>> packages);

        private void ApplyResults()
        {
            Tmc.Refresh(Items);
            view.SetItems(Items, Tmc);
            if (Config.UsePrefetch)
            {
                prefetchManager.SaveStatistic(Config.TestDllPath, Tmc.Tests);
            }
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
