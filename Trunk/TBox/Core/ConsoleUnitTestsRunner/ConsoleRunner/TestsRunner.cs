using System;
using System.IO;
using Common.Base.Log;
using Common.MT;
using ConsoleUnitTestsRunner.Code;
using ConsoleUnitTestsRunner.Code.Updater;

namespace ConsoleUnitTestsRunner.ConsoleRunner
{
	class TestsRunner
	{
		private static readonly ILog Log = LogManager.GetLogger<TestsRunner>();

        public int Run(string path, int nCores, bool x86, bool cloneTests, int cloneDeep, bool needSynchronization, bool needReport, string dirToCloneTests, string commandToExecuteBeforeTests)
		{
			var time = Environment.TickCount;
			var view = new ConsoleView();
			using (var p = new TestsPackage(path, "NUnitAgent.exe", x86, false, dirToCloneTests, commandToExecuteBeforeTests, view))
			{
				Console.WriteLine("Calculating tests..");
				if (!p.EnsurePathIsValid())
				{
					Log.Write("Incorrect path: " + path);
					return -3;
				}
				var error = false;
				p.DoRefresh(x => { }, x => error = true);
				Console.WriteLine("{0} tests founded", p.Count);
				if (error)
				{
					Log.Write("Can't calculate tests count");
					return -3;
				}
				Console.WriteLine("Running tests..");
				var packages = p.PrepareToRun(nCores);
				var updater = new ConsoleUpdater();
				var synchronizer = new Synchronizer(nCores);
				p.DoRun(x=>PrintInfo(x, time, view, needReport, path), 
					packages, cloneTests, cloneDeep, needSynchronization, synchronizer, new SimpleUpdater(updater, synchronizer));
				Console.WriteLine("Done");
				return (p.FailedCount > 0) ? -2 : 0;
			}
		}

		private static void PrintInfo(TestsPackage package, int time, ConsoleView view, bool needReport,string path)
		{
			package.ApplyResults();
			if (needReport) view.GenerateReport(Environment.TickCount - time, path);
			Console.WriteLine("{0} - tests: [ {1} ], failed = [ {2} ] time: {3}",
											Path.GetFileName(package.Path),
											package.Count, 
											package.FailedCount,
											(Environment.TickCount - time )/ 1000);
		}
	}
}
