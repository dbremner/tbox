using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Mnk.Library.ParallelNUnit.Contracts;
using NUnit.Core;
using ServiceStack.Text;

namespace Mnk.Library.ParallelNUnit.Core
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class NunitRunnerClient : INunitRunnerClient
    {
        private readonly object locker = new object();
        public Result[] Collection { get; private set; }
        private readonly IDictionary<int, Result> allTestsResults;
        private int allTestsCount;
        public IRunnerContext TestsRunnerContext { get; private set; }
        private TestRunConfig runConfig;
        private ISynchronizer synchronizer;
        private ITestsUpdater progress;

        public NunitRunnerClient()
        {
            Collection = new Result[0];
            allTestsResults = new Dictionary<int, Result>();
            TestsRunnerContext = null;
        }

        public void PrepareToCalc()
        {
            Collection = new Result[0];
        }

        public void PrepareToRun(ISynchronizer sync, ITestsUpdater u, TestRunConfig config, IList<IList<Result>> packages, IList<Result> allTests , IRunnerContext runnerContext, ITestsMetricsCalculator metricsCalculator)
        {
            runConfig = config;
            allTestsCount = metricsCalculator.Total;
            allTestsResults.Clear();
            synchronizer = sync;
            progress = u;
            foreach (var package in packages)
            {
                runConfig.TestsToRun.Add(package.Select(x => x.Id).ToArray());
            }
            FillAllTests(allTests);
            TestsRunnerContext = runnerContext;
        }

        private void FillAllTests(IEnumerable<Result> allTests)
        {
            foreach (var item in allTests)
            {
                allTestsResults[item.Id] = item;
                FillAllTests(item.Children.Cast<Result>());
            }
        }

        public void SetCollectedTests(string itemsText)
        {
            Collection = new[] { JsonSerializer.DeserializeFromString<Result>(itemsText) };
        }

        public string GiveMeConfig()
        {
            lock (locker)
            {
                if (runConfig!=null)
                {
                    var ret = JsonSerializer.SerializeToString(runConfig);
                    runConfig = null;
                    return ret;
                }
                return null;
            }
        }

        public bool SendTestsResults(string itemsText)
        {
            var items = JsonSerializer.DeserializeFromString<Result[]>(itemsText);
            var failed = 0;
            lock (allTestsResults)
            {
                foreach (var i in items)
                {
                    Result item;
                    if(!allTestsResults.TryGetValue(i.Id, out item))continue;
                    item.Key = i.Key;
                    item.Time = Math.Max(item.Time, i.Time);

                    if (i.IsTest)
                    {
                        Map(item, i);
                        if (IsFailed(i))
                        {
                            ++failed;
                        }
                    }
                    else
                    {
                        if (!IsFailed(item) && !(item.State == ResultState.Success && i.State == ResultState.Inconclusive))
                        {
                            Map(item, i);
                        }
                    }
                }
            }
            progress.Update(allTestsCount, items.Where(x => x.IsTest).ToArray(), failed, synchronizer, runConfig.Config);
            return progress.UserPressClose;
        }

        private static void Map(Result item, Result i)
        {
            item.AssertCount = i.AssertCount;
            item.Executed = i.Executed;
            item.Description = i.Description;
            item.Message = i.Message;
            item.StackTrace = i.StackTrace;
            item.Output = i.Output;
            item.State = i.State;
            item.Refresh();
        }

        private static bool IsFailed(Result i)
        {
            return i.State == ResultState.Error || i.State == ResultState.Failure;
        }

        public void CanFinish(string handle)
        {
            synchronizer.ProcessNextAgent(runConfig.Config, handle);
        }
    }
}
