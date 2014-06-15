using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly object sync = new object();
        public Result[] Collection { get; private set; }
        private readonly IDictionary<int, Result> allTestsResults;
        private int allTestsCount;
        private TestRunConfig runConfig;
        private ISynchronizer synchronizer;
        private ITestsUpdater progress;
        private ITestsConfig testsConfig;

        public NunitRunnerClient()
        {
            Collection = new Result[0];
            allTestsResults = new Dictionary<int, Result>();
        }

        public void PrepareToCalc()
        {
            Collection = new Result[0];
        }

        public void PrepareToRun(ISynchronizer synchro, ITestsUpdater u, TestRunConfig config, IList<IList<Result>> packages, IList<Result> allTests , ITestsMetricsCalculator metricsCalculator, ITestsConfig testConfig)
        {
            testsConfig = testConfig;
            runConfig = config;
            allTestsCount = metricsCalculator.Total;
            allTestsResults.Clear();
            synchronizer = synchro;
            progress = u;
            foreach (var package in packages)
            {
                runConfig.TestsToRun.Add(package.Select(x => x.Id).ToArray());
            }
            FillAllTests(allTests);
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
            lock (sync)
            {
                if (runConfig == null)
                    throw new ArgumentNullException("TestRunConfig can't be null. Did you forget to execute PrepareToRun?");
                if (string.Equals(testsConfig.Mode, TestsRunnerMode.MultiProcess))
                {
                    if (runConfig.DllPaths.Count > 1)
                    {
                        var cfg = new TestRunConfig(new[] {runConfig.DllPaths[0]});
                        cfg.TestsToRun.Add(runConfig.TestsToRun[0]);
                        runConfig.DllPaths.RemoveAt(0);
                        runConfig.TestsToRun.RemoveAt(0);
                        return JsonSerializer.SerializeToString(cfg);
                    }
                }
                var ret = JsonSerializer.SerializeToString(runConfig);
                runConfig = null;
                return ret;
            }
        }

        public bool SendTestsResults(string itemsText)
        {
            DoSendResults(itemsText);
            return progress.UserPressClose;
        }

        private void DoSendResults(string itemsText)
        {
            var items = JsonSerializer.DeserializeFromString<Result[]>(itemsText);
            var failed = 0;
            lock (allTestsResults)
            {
                foreach (var i in items)
                {
                    Result item;
                    if (!allTestsResults.TryGetValue(i.Id, out item)) continue;
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
            progress.Update(allTestsCount, items.Where(x => x.IsTest).ToArray(), failed, synchronizer, testsConfig);
        }

        private static void Map(Result item, Result i)
        {
            item.AssertCount = i.AssertCount;
            item.Description = i.Description;
            item.Message = i.Message;
            item.StackTrace = Filter(i.StackTrace);
            item.Output = i.Output;
            item.State = i.State;
            item.FailureSite = i.FailureSite;
            item.Refresh();
        }

        public static string Filter(string stack)
        {
            if (stack == null) return null;
            using (var sw = new StringWriter())
            {
                using (var sr = new StringReader(stack))
                {
                    try
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!FilterLine(line))
                            {
                                sw.WriteLine(line.Trim());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return stack;
                    }
                    return sw.ToString();
                }
            }
        }

        static bool FilterLine(string line)
        {
            var patterns = new[]
			{
				"NUnit.Core.TestCase",
				"NUnit.Core.ExpectedExceptionTestCase",
				"NUnit.Core.TemplateTestCase",
				"NUnit.Core.TestResult",
				"NUnit.Core.TestSuite",
				"NUnit.Framework.Assertion", 
				"NUnit.Framework.Assert",
                "System.Reflection.MonoMethod",
                "Mnk.Library.ParallelNunit",
			};

            return patterns.Any(t => line.IndexOf(t, StringComparison.Ordinal) > 0);
        }

        private static bool IsFailed(Result i)
        {
            return i.State == ResultState.Error || i.State == ResultState.Failure;
        }

        public void CanFinish(string handle)
        {
            synchronizer.ProcessNextAgent(testsConfig, handle);
        }
    }
}
