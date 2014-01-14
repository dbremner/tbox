using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Execution;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Infrastructure.Runners;
using Mnk.Library.ParallelNUnit.Interfaces;
using NUnit.Core;
using ServiceStack.Text;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Communication
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class NunitRunnerClient : INunitRunnerClient
    {
        private readonly object locker = new object();
        public Result[] Collection { get; private set; }
        private readonly IDictionary<int, Result> allTestsResults;
        private int allTestsCount;
        public IContext TestsContext { get; private set; }
        private TestRunConfig runConfig;
        private Synchronizer synchronizer;
        private IProgressStatus progress;

        public NunitRunnerClient()
        {
            Collection = new Result[0];
            allTestsResults = new Dictionary<int, Result>();
            TestsContext = null;
        }

        public void PrepareToCalc()
        {
            Collection = new Result[0];
        }

        public void PrepareToRun(Synchronizer sync, IProgressStatus u, TestRunConfig config, IList<IList<Result>> packages, IList<Result> allTests , IContext context)
        {
            runConfig = config;
            allTestsCount = 0;
            allTestsResults.Clear();
            synchronizer = sync;
            progress = u;
            foreach (var package in packages)
            {
                runConfig.TestsToRun.Add(package.Select(x => x.Id).ToArray());
                allTestsCount += package.Count;
            }
            FillAllTests(allTests);
            TestsContext = context;
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
                        if (!IsFailed(item))
                        {
                            Map(item, i);
                        }
                    }
                }
            }
            progress.Update(allTestsCount, items.Where(x=>x.IsTest).ToArray(), failed);
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
        }

        private static bool IsFailed(Result i)
        {
            return i.State == ResultState.Error || i.State == ResultState.Failure;
        }

        public void CanFinish(string handle)
        {
            synchronizer.ProcessNextAgent(handle);
        }
    }
}
