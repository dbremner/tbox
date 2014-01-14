using System;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Packages;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    class TestsRunner
    {
        private static readonly ILog Log = LogManager.GetLogger<TestsRunner>();

        public int Run(CommandLineArgs a)
        {
            var time = Environment.TickCount;
            var view = new ConsoleView();
            var dir = Environment.CurrentDirectory;
            using (var p = new ThreadPackage(a.Path, a.DirToCloneTests, a.CommandBeforeTestsRun, view, Program.LoadFromSameFolder))
            {
                if (a.Logo) Console.WriteLine("Calculating tests..");
                if (!p.EnsurePathIsValid())
                {
                    Log.Write("Incorrect path: " + a.Path);
                    return -3;
                }
                var error = false;
                p.DoRefresh(x => { }, x => error = true);
                if (a.Logo) Console.WriteLine("{0} tests founded", p.Count);
                if (error)
                {
                    Log.Write("Can't calculate tests count");
                    return -3;
                }
                if (a.Logo) Console.WriteLine("Running tests..");
                var packages = p.PrepareToRun(a.ProcessCount, a.Include ?? a.Exclude, a.Include != null && a.Exclude != null, a.Prefetch);
                var updater = new ConsoleUpdater();
                var synchronizer = new Synchronizer(a.ProcessCount);
                p.DoRun(x => PrintInfo(x, time, view, a.XmlReport, a.OutputReport, a.Path, dir, a.Prefetch),
                    p.Items, packages, a.Clone, a.CopyMasks, a.Sync, a.StartDelay, synchronizer, BuildUpdater(a.Labels, a.Teamcity, updater, synchronizer), !string.IsNullOrEmpty(a.OutputReport));
                return (p.FailedCount > 0) ? -2 : 0;
            }
        }

        private static SimpleUpdater BuildUpdater(bool labels, bool teamcity, IUpdater updater, Synchronizer synchronizer)
        {
            if(teamcity)return new TeamcityUpdater(updater, synchronizer);
            return labels ? 
                new NUnitLabelsUpdater(updater, synchronizer) : 
                new SimpleUpdater(updater, synchronizer);
        }

        private static void PrintInfo(IPackage package, int time, ConsoleView view, string xmlReport, string outputReport, string path, string dir, bool usePrefetch)
        {
            view.Time = (Environment.TickCount - time)/1000.0;
            package.ApplyResults(usePrefetch);
            Directory.SetCurrentDirectory(dir);
            if (!string.IsNullOrEmpty(xmlReport)) view.GenerateReport(path, xmlReport);
            if (!string.IsNullOrEmpty(outputReport))
            {
                File.WriteAllText(
                    Path.Combine(Environment.CurrentDirectory, outputReport), 
                    string.Join(string.Empty, view.Tmc.All.Select(x=>x.Output)));
            }
        }
    }
}
