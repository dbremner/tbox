﻿using System;
using System.IO;
using System.Linq;
using Common.Base.Log;
using Common.MT;
using Common.Tools;
using PluginsShared.UnitTests;
using PluginsShared.UnitTests.Settings;
using PluginsShared.UnitTests.Updater;

namespace ConsoleUnitTestsRunner.ConsoleRunner
{
	class TestsRunner
	{
		private static readonly ILog Log = LogManager.GetLogger<TestsRunner>();

        public int Run(string path, int nCores, bool x86, bool cloneTests, string[] copyMasks, bool needSynchronization, bool needReport, string dirToCloneTests, string commandToExecuteBeforeTests, string[] include, string[] exclude, int startDelay)
		{
			var time = Environment.TickCount;
			var view = new ConsoleView();
            using (var p = new TestsPackage(path, "NUnitAgent.exe", x86, false, dirToCloneTests, commandToExecuteBeforeTests, view, "RunAsx86.exe"))
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
				var packages = p.PrepareToRun(nCores, include??exclude, include!=null && exclude!=null);
				var updater = new ConsoleUpdater();
				var synchronizer = new Synchronizer(nCores);
				p.DoRun(x=>PrintInfo(x, time, view, needReport, path),
                    packages, cloneTests, copyMasks, needSynchronization, startDelay, synchronizer, new SimpleUpdater(updater, synchronizer));
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
