using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Base.Log;
using Common.Communications;
using Common.UI.ModelsContainers;
using NUnit.Core;
using NUnitRunner.Code.Communication;
using NUnitRunner.Code.Updater;
using NUnitRunner.Components;
using extended.nunit.Interfaces;
using Result = NUnitRunner.Code.Settings.Result;

namespace NUnitRunner.Code
{
	class TestsPackage : IDisposable
	{
	    private bool runAsx86;
		private bool runAsAdmin;
		private static readonly ILog Log = LogManager.GetLogger<TestsPackage>();
		
		public UnitTestsView Results { get; private set; }
		public string Path { get; private set; }
		private CheckableDataCollection<Result> items;
		private readonly Server<INunitRunnerClient> server;
		private readonly Runner runner;
		private readonly Calculator calculator;
		public int Count { get { return items.Count; } }
		public int FailedCount{get{return items.Count(x => x.State == ResultState.Failure || x.State == ResultState.Error);}}

		public TestsPackage(string path, string nunitAgentPath, bool runAsx86, bool runAsAdmin)
		{
		    this.runAsx86 = runAsx86;
			this.runAsAdmin = runAsAdmin;
			Path = path;
			server = new Server<INunitRunnerClient>(new NunitRunnerClient());
            calculator = new Calculator(nunitAgentPath);
			runner = new Runner(nunitAgentPath);
			items = new CheckableDataCollection<Result>();
			Results = new UnitTestsView();
		}

		public bool EnsurePathIsValid()
		{
			if (File.Exists(Path)) return true;
			MessageBox.Show("Can't load dll: " + Path);
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
				runner.Run(Path, packages, server, copyToSeparateFolders, copyDeep, needSynchronizationForTests, runAsx86, runAsAdmin, synchronizer, u);
				onReceive(this);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't run test, from dll: " + Path);
			}
		}

		public void UpdateFilter(bool onlyFailed)
		{
			if (Results == null || Results.ItemsSource == null) return;
			Results.OnlyFailed = onlyFailed;
		}

		public void ApplyResults()
		{
			Results.ItemsSource = items;
		}

		public void Dispose()
		{
			server.Dispose();
		}

	    public void Reset(bool runAsx86, bool runAsAdmin)
	    {
	        this.runAsx86 = runAsx86;
		    this.runAsAdmin = runAsAdmin;
	        Results.ItemsSource = null;
	    }
	}
}
