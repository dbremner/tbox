using System;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Packages;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    internal class TestsRunner
    {
        private static readonly ILog Log = LogManager.GetLogger<TestsRunner>();

        public static int Run(CommandLineArgs args)
        {
            var view = new ConsoleView();
            var dir = Environment.CurrentDirectory;

            var retValue = 0;

            foreach (var path in args.Paths)
            {
                using (
                    var p = new ThreadPackage(
                        path,
                        args.DirToCloneTests,
                        args.CommandBeforeTestsRun,
                        view,
                        Program.LoadFromSameFolder,
                        args.RuntimeFramework))
                {
                    view.NotifyNewAssemblyStartTest();

                    Console.WriteLine("Running tests for {0}.", path);
                    if (args.Logo) Console.WriteLine("Calculating tests.");
                    if (!p.EnsurePathIsValid())
                    {
                        Log.Write("Incorrect path: " + path);
                        retValue = -3;
                        continue;
                    }

                    var error = false;
                    p.DoRefresh(x => { }, x => error = true);
                    if (args.Logo) Console.WriteLine("{0} tests found.", p.Count);
                    if (error)
                    {
                        Log.Write("Can't calculate tests count");
                        retValue = -3;
                        continue;
                    }

                    if (args.Logo) Console.WriteLine("Running tests.");
                    var packages = p.PrepareToRun(
                        args.ProcessCount,
                        args.Include ?? args.Exclude,
                        args.Include != null && args.Exclude != null,
                        args.Prefetch);
                    using (var updater = new ConsoleUpdater())
                    {
                        var synchronizer = new Synchronizer(args.ProcessCount);
                        p.DoRun(
                            package => package.ApplyResults(args.Prefetch),
                            p.Items,
                            packages,
                            args.Clone,
                            args.CopyMasks,
                            args.Sync,
                            args.StartDelay,
                            synchronizer,
                            BuildUpdater(args.Labels, args.Teamcity, updater, synchronizer),
                            !string.IsNullOrEmpty(args.OutputReport));
                        if (p.FailedCount > 0) retValue = -2;
                    }
                }
            }

            view.PrintTotalResults();
            PrintTotalInfo(view, args.XmlReport, args.OutputReport, args.Paths.FirstOrDefault(), dir);
            
            return retValue;
        }

        private static SimpleUpdater BuildUpdater(bool labels, bool teamcity, IUpdater updater, Synchronizer synchronizer)
        {
            if(teamcity)return new TeamcityUpdater(updater, synchronizer);
            return labels ? 
                new NUnitLabelsUpdater(updater, synchronizer) : 
                new SimpleUpdater(updater, synchronizer);
        }

        private static void PrintTotalInfo(ConsoleView view, string xmlReport, string outputReport, string path, string dir)
        {
            Directory.SetCurrentDirectory(dir);
            if (!string.IsNullOrEmpty(xmlReport)) view.GenerateReport(path, xmlReport);
            if (!string.IsNullOrEmpty(outputReport))
            {
                var totalResult = view.CreateTotalResult();
                File.WriteAllText(
                    Path.Combine(Environment.CurrentDirectory, outputReport),
                    string.Join(string.Empty, totalResult.Metrics.All.Select(x => x.Output)));
            }
        }
    }
}
