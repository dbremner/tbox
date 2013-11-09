using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Common.Base.Log;
using extended.nunit.Interfaces;
using NUnit.Core;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Settings;
using ServiceStack.Text;

namespace PluginsShared.UnitTests.Communication
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
	class NunitRunnerClient : INunitRunnerClient
	{
		private ILog Log = LogManager.GetLogger<NunitRunnerClient>();
		public Result[] Collection { get; private set; }
		public List<IEnumerable<int>> TestsToRun { get; private set; }
		public IDictionary<int, Result> AllTestsResults { get; private set; }
		public List<Process> Processes { get; private set; }
		private Synchronizer synchronizer;
		private IProgressStatus progress;

		public NunitRunnerClient()
		{
			Collection = new Result[0];
			TestsToRun = new List<IEnumerable<int>>();
			AllTestsResults = new Dictionary<int, Result>();
			Processes = new List<Process>();
		}

		public void PrepareToCalc()
		{
			Collection = new Result[0];
		}

		public void PrepareToRun(Synchronizer sync, IProgressStatus u)
		{
			TestsToRun.Clear();
			AllTestsResults.Clear();
			Processes.Clear();
			synchronizer = sync;
			progress = u;
		}

		public void SetCollectedTests(string results)
		{
			Collection = JsonSerializer.DeserializeFromString<Result[]>(results);
		}

		public string GiveMeTestIds()
		{
			IEnumerable<int> ret;
			lock (TestsToRun)
			{
				if (TestsToRun.Any())
				{
					ret = TestsToRun[0];
					TestsToRun.RemoveAt(0);
				}
				else
				{
					ret = new int[0];
				}
			}
			return string.Join(",", ret);
		}

		public void SendTestsResults(string itemsText)
		{
			var failed = 0;
			var items = JsonSerializer.DeserializeFromString<Result[]>(itemsText);
			foreach (var i in items )
			{
				var item = AllTestsResults[i.Id];
				item.Message = i.Message;
				item.StackTrace = i.StackTrace;
				item.State = i.State;
				if (i.State == ResultState.Error || i.State == ResultState.Failure)
				{
					++failed;
				}
			}
			progress.Update(AllTestsResults.Count, items.Length, failed);
			if (!progress.UserPressClose) return;
			try
			{
				foreach (var p in Processes)
				{
					p.Kill();
				}
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error stopping tests");
			}

		}

		public void CanFinish(string handle)
		{
			synchronizer.ProcessNextAgent(handle);
		}
	}
}
