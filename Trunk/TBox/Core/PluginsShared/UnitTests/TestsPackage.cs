using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Base.Log;
using Common.Communications.Interprocess;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using extended.nunit.Interfaces;
using NUnit.Core;
using PluginsShared.UnitTests.Communication;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Settings;

namespace PluginsShared.UnitTests
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
		private readonly InterprocessServer<INunitRunnerClient> server;
		private readonly Runner runner;
		private readonly Calculator calculator;
		public int Count { get { return items.Count; } }
		public int FailedCount{get{return items.Count(x => x.State == ResultState.Failure || x.State == ResultState.Error);}}

	    public CheckableDataCollection<CheckableData> Categories
	    {
	        get
	        {
	            return new CheckableDataCollection<CheckableData>(
                    items
                    .SelectMany(x => x.Categories)
                    .Distinct()
                    .OrderBy(x=>x)
                    .Select(x => new CheckableData {Key = x})
                    );
	        }
	    }

	    public TestsPackage(string path, string nunitAgentPath, bool runAsx86, bool runAsAdmin, string dirToCloneTests, string commandToExecuteBeforeTests, IUnitTestsView view, string runAsx86Path)
		{
		    this.runAsx86 = runAsx86;
			this.runAsAdmin = runAsAdmin;
            this.dirToCloneTests = dirToCloneTests;
            this.commandToExecuteBeforeTests = commandToExecuteBeforeTests;
			Path = path;
			server = new InterprocessServer<INunitRunnerClient>(new NunitRunnerClient());
            calculator = new Calculator(nunitAgentPath, runAsx86Path);
			runner = new Runner(nunitAgentPath, runAsx86Path);
			items = new CheckableDataCollection<Result>();
			Results = view;
		}

		public bool EnsurePathIsValid()
		{
			if (File.Exists(Path)) return true;
			Log.Write("Can't load dll: " + Path);
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

		public IList<IList<Result>> PrepareToRun(int processCount, string[] categories, bool?include)
		{
			foreach (var i in items)
			{
				i.State = ResultState.Inconclusive;
			}
            var filter = GetFilter(categories, include);
		    return (processCount > 1)
                                ? runner.DivideTestsToRun(items.CheckedItems.Where(filter).ToArray(), processCount).ToArray()
								: new[] { items.CheckedItems.Where(filter).ToArray() };
		}

	    private static Func<Result, bool> GetFilter(string[] categories, bool? include)
	    {
	        if (include.HasValue && categories.Length > 0)
	        {
	            if (include.Value)
	            {
	                return r => IncludeFilter(r, categories);
	            }
	            return r => ExcludeFilter(r, categories);
	        }
	        return r=>true;
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
