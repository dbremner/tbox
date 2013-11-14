using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Communications.Interprocess;
using NUnit.Core;
using Common.Communications;
using ServiceStack.Text;
using extended.nunit.Interfaces;

namespace extended.nunit
{
	[Serializable]
	public sealed class RemoteListener : EventListener
	{
		public string Handle { get; set; }
		public bool Fast { get; set; }
		private int time = Environment.TickCount;
		private Client<INunitRunnerClient> client;
		private readonly IList<Result> items = new List<Result>();
		private int expectedTestCount;

		private Client<INunitRunnerClient> GetClient()
		{
			return client ?? (client = new Client<INunitRunnerClient>(Handle));
		}

		public void RunStarted(string name, int testCount)
		{
			expectedTestCount = testCount-1;
		}

		public void RunFinished(TestResult result)
		{
			SendAll();
		}

		public void RunFinished(Exception exception)
		{
		}

		public void TestStarted(TestName testName)
		{
		}

		public void TestFinished(TestResult result)
		{
			items.Add(new Result
				{
					Id = int.Parse(result.Test.TestName.TestID.ToString()),
					Message = result.Message,
					StackTrace = result.StackTrace,
					State = result.ResultState,
				});
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

		public void SuiteStarted(TestName testName)
		{
		}

		public void SuiteFinished(TestResult result)
		{
		}

		public void UnhandledException(Exception exception)
		{
		}

		public void TestOutput(TestOutput testOutput)
		{
		}

		private void WaitUntillAllOtherTestsFinished()
		{
			var s = new NunitRunnerServer();
			using (var server = new Server<INunitRunnerServer>(s))
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
			if (items.Count == 0)return;
			GetClient().Instance.SendTestsResults(JsonSerializer.SerializeToString(items.ToArray()));
			items.Clear();
		}
	}
}
