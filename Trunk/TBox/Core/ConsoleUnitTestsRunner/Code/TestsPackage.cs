using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Base.Log;
using Common.Communications;
using Common.UI.ModelsContainers;
using ConsoleUnitTestsRunner.Code.Communication;
using ConsoleUnitTestsRunner.Code.Interfaces;
using ConsoleUnitTestsRunner.Code.Settings;
using NUnit.Core;
using extended.nunit.Interfaces;

namespace ConsoleUnitTestsRunner.Code
{
	public sealed class TestsPackage : IDisposable
	{
	    private bool runAsx86;
		private bool runAsAdmin;
	    private string dirToCloneTests;
	    private string commandToExecuteBeforeTests;
		private static readonly ILog Log = LogManager.GetLogger<TestsPackage>();
		
		public IUnitTestsView Results { get; private set; }
		public string Path { get; private set; }
		private CheckableDataCollection<Result> items;
		private readonly Server<INunitRunnerClient> server;
		private readonly Runner runner;
		private readonly Calculator calculator;
		public int Count { get { return items.Count; } }
		public int FailedCount{get{return items.Count(x => x.State == ResultState.Failure || x.State == ResultState.Error);}}

        public TestsPackage(string path, string nunitAgentPath, bool runAsx86, bool runAsAdmin, string dirToCloneTests, string commandToExecuteBeforeTests, IUnitTestsView view)
		{
		    this.runAsx86 = runAsx86;
			this.runAsAdmin = runAsAdmin;
            this.dirToCloneTests = dirToCloneTests;
            this.commandToExecuteBeforeTests = commandToExecuteBeforeTests;
			Path = path;
			server = new Server<INunitRunnerClient>(new NunitRunnerClient());
            calculator = new Calculator(nunitAgentPath);
			runner = new Runner(nunitAgentPath);
			items = new CheckableDataCollection<Result>();
			Results = view;
		}

		public bool EnsurePathIsValid()
		{
			if (File.Exists(Path)) return true;
			Log.Write("Can't load dll: " + Path);
			return false;
		}

		public IList<IList<Result>> PrepareToRun(int processCount)
		{
			foreach (var i in items)
			{
				i.State = ResultState.Inconclusive;
			}
			return (processCount > 1)
								? runner.DivideTestsToRun(items.CheckedItems.ToArray(), processCount).ToArray()
								: new[] { items.CheckedItems.ToArray() };
		}

		public void DoRefresh(Action<TestsPackage> onReceive, Action<TestsPackage> onError)
		{
			try
			{
				var client = ((NunitRunnerClient) server.Owner);
				client.PrepareToCalc();
				calculator.CollectTests(Path, runAsx86, false, server.Handle);
				items = new CheckableDataCollection<Result>(client.Collection);
				onReceive(this);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't refresh tests from dll: " + Path);
				onError(this);
			}
		}

		public void DoRun(Action<TestsPackage> onReceive, IList<IList<Result>> packages, bool copyToSeparateFolders, int copyDeep, bool needSynchronizationForTests, Synchronizer synchronizer, IProgressStatus u)
		{
			try
			{
                runner.Run(Path, packages, server, copyToSeparateFolders, copyDeep, needSynchronizationForTests, runAsx86, runAsAdmin, dirToCloneTests, commandToExecuteBeforeTests, synchronizer, u);
				onReceive(this);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't run test, from dll: " + Path);
			}
		}

		public void UpdateFilter(bool onlyFailed)
		{
			if (Results!=null) Results.UpdateFilter(onlyFailed);
		}

		public void ApplyResults()
		{
			Results.SetItems(items);
		}

		public void Dispose()
		{
			server.Dispose();
		}

        public void Reset(bool asx86, bool asAdmin, string dirToClone, string commandBeforeTests)
	    {
	        runAsx86 = asx86;
		    runAsAdmin = asAdmin;
            dirToCloneTests = dirToClone;
            commandToExecuteBeforeTests = commandBeforeTests;
	        Results.Clear();
	    }
	}
}
