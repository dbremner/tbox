using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure.Communication;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Interfaces;
using NUnit.Core;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Packages
{
    public abstract class BasePackage : IPackage
    {
        protected InterprocessServer<INunitRunnerClient> Server { get; private set; }
        protected string DirToCloneTests { get; private set; }
        protected string CommandToExecuteBeforeTests { get; private set; }
        protected ILog Log { get; private set; }
        private IList<Result> collected;

        public IUnitTestsView Results { get; private set; }
        public string FilePath { get; private set; }
        public string RuntimeFramework { get; private set; }
        public IList<Result> Items
        {
            get { return collected; }
            set
            {
                collected = value;
                Metrics = new TestsMetricsCalculator(Items);
            }
        }

        protected TestsMetricsCalculator Metrics { get; private set; }
        private readonly PrefetchManager prefetchManager;
        public int Count { get { return Metrics.Total; } }
        public int FailedCount { get { return Metrics.Failed.Length; } }

        protected BasePackage(string path, string dirToCloneTests, string commandToExecuteBeforeTests, IUnitTestsView view, string runtimeFramework)
        {
            Server = new InterprocessServer<INunitRunnerClient>(new NunitRunnerClient());
            DirToCloneTests = dirToCloneTests;
            CommandToExecuteBeforeTests = commandToExecuteBeforeTests;
            FilePath = path;
            RuntimeFramework = runtimeFramework;
            Items = new Result[0];
            Results = view;
            prefetchManager = new PrefetchManager();
            Log = LogManager.GetLogger<BasePackage>();
        }

        public bool EnsurePathIsValid()
        {
            if (File.Exists(FilePath)) return true;
            Log.Write("Can't load dll: " + FilePath);
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

        public IList<IList<Result>> PrepareToRun(int processCount, string[] categories, bool? include, bool usePrefetch, IList<Result> checkedTests=null)
        {
            ResetTests(checkedTests);
            var items = checkedTests ?? Metrics.Tests;
            var filter = GetFilter(categories, include);
            return (processCount > 1)
                                ? DivideTestsToRun(items.Where(filter).ToArray(), processCount, usePrefetch).ToArray()
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

        public IEnumerable<IList<Result>> DivideTestsToRun(IList<Result> tests, int threadCount, bool usePrefetch)
        {
            if (usePrefetch && threadCount > 1)
            {
                tests = prefetchManager.Optimize(FilePath,tests);
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

        public abstract void DoRefresh(Action<IPackage> onReceive, Action<IPackage> onError);
        public abstract void DoRun(Action<IPackage> onReceive, IList<Result> allTests, IList<IList<Result>> packages,
                                   bool copyToSeparateFolders, string[] copyMasks, bool needSynchronizationForTests,
                                   int startDelay, Synchronizer synchronizer, IProgressStatus updater, bool needOutput);

        public void ApplyResults(bool usePrefetch)
        {
            Metrics = new TestsMetricsCalculator(Items);
            Results.SetItems(Items, Metrics);
            if (usePrefetch)
            {
                prefetchManager.SaveStatistic(FilePath, Metrics.Tests);
            }
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
