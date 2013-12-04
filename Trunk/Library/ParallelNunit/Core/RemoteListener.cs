using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Communications.Interprocess;
using NUnit.Core;
using ParallelNUnit.Execution;
using ParallelNUnit.Interfaces;
using ServiceStack.Text;

namespace ParallelNUnit.Core
{
    [Serializable]
    public sealed class RemoteListener : EventListener
    {
        public readonly StringBuilder Output = new StringBuilder();
        public static bool ShouldStop = false;
        public string Handle { get; set; }
        public bool Fast { get; set; }
        public bool Needoutput { get; set; }

        private int time = Environment.TickCount;
        private InterprocessClient<INunitRunnerClient> client;
        private readonly IList<Result> items = new List<Result>();
        private int expectedTestCount;

        private InterprocessClient<INunitRunnerClient> GetClient()
        {
            return client ?? (client = new InterprocessClient<INunitRunnerClient>(Handle));
        }

        public void RunStarted(string name, int testCount)
        {
            expectedTestCount = testCount - 1;
        }

        public void RunFinished(TestResult result)
        {
            if (Needoutput)
            {
                GetClient().Instance.SendOutput(Output.ToString());
            }
            SendAll();
            client.Dispose();
        }

        public void RunFinished(Exception exception)
        {
        }

        public void TestStarted(TestName testName)
        {
        }

        public void TestFinished(TestResult result)
        {
            SaveResult(result);
            if (!Fast)
            {
                if (--expectedTestCount == 0)
                {
                    SendAll();
                    WaitUntillAllOtherTestsFinished();
                    return;
                }
            }
            if (Environment.TickCount - time <= 3000) return;
            SendAll();
            time = Environment.TickCount;
        }

        private void SaveResult(TestResult result)
        {
            items.Add(new Result
                {
                    Id = int.Parse(result.Test.TestName.TestID.ToString()),
                    Key = result.Test.TestName.Name,
                    FullName = result.Test.TestName.FullName,
                    Description = result.Description,
                    Message = result.Message,
                    StackTrace = result.StackTrace,
                    State = result.ResultState,
                    Time = result.Time,
                    Type = result.Test.TestType,
                    Executed = result.Executed,
                    AssertCount = result.AssertCount,
                });
        }

        public void SuiteStarted(TestName testName)
        {
        }

        public void SuiteFinished(TestResult result)
        {
            SaveResult(result);
        }

        public void UnhandledException(Exception exception)
        {
        }

        public void TestOutput(TestOutput testOutput)
        {
            if (Needoutput) Output.AppendLine(testOutput.Text);
        }

        private void WaitUntillAllOtherTestsFinished()
        {
            var s = new NUnitRunnerServer();
            using (var server = new InterprocessServer<INunitRunnerServer>(s))
            {
                GetClient().Instance.CanFinish(server.Handle);
                while (s.ShouldWait)
                {
                    Thread.Sleep(20);
                }
            }
        }

        private void SendAll()
        {
            if (items.Count == 0) return;
            var str = JsonSerializer.SerializeToString(items.ToArray());
            ShouldStop = GetClient().Instance.SendTestsResults(str);
            items.Clear();
        }
    }
}
