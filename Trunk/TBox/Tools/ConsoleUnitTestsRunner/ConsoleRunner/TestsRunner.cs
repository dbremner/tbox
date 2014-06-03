﻿using System;
using System.IO;
using System.Linq;
using LightInject;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    internal class TestsRunner
    {
        private readonly ILog log = LogManager.GetLogger<TestsRunner>();

        public int Run(CommandLineArgs args)
        {
            using (var container = ServicesRegistrar.Register())
            {
                return Execute(args, container);
            }
        }

        private int Execute(CommandLineArgs args,  IServiceContainer container)
        {
            int retValue = 0;
            var view = new ConsoleView();
            using (var updater = new ConsoleUpdater())
            {
                var testsUpdater = BuildUpdater(args.Labels, args.Teamcity, updater);
                foreach (var path in args.Paths)
                {
                    var config = CreateConfig(args, path);
                    retValue = Math.Min(retValue, RunTest(args, path, container, view, config, testsUpdater));
                }
            }

            view.PrintTotalResults();
            PrintTotalInfo(view, args.XmlReport, args.OutputReport, args.Paths.FirstOrDefault(),
                Environment.CurrentDirectory);
            return retValue;
        }

        private int RunTest(CommandLineArgs args, string path, IServiceContainer container, ConsoleView view, IThreadTestConfig config, ITestsUpdater testsUpdater)
        {
            var p = container.GetInstance<IPackage<IThreadTestConfig>>();

            view.NotifyNewAssemblyStartTest();

            Console.WriteLine("Running tests for {0}.", path);
            if (args.Logo) Console.WriteLine("Calculating tests.");
            if (!p.EnsurePathIsValid(config))
            {
                log.Write("Incorrect path: " + path);
                return -3;
            }

            var results = p.Refresh(config);
            if (args.Logo) Console.WriteLine("{0} tests found.", results.Metrics.Total);
            if (results.IsFailed)
            {
                log.Write("Can't calculate tests count");
                return -3;
            }

            if (args.Logo) Console.WriteLine("Running tests.");
            results = p.Run(config, results, testsUpdater);
            view.SetItems(results.Items, results.Metrics);
            if (results.Metrics.FailedCount > 0) return -2;
            return 0;
        }

        private static IThreadTestConfig CreateConfig(CommandLineArgs args, string path)
        {
            return new ThreadTestConfig
            {
                CopyMasks = args.CopyMasks,
                CommandBeforeTestsRun = args.CommandBeforeTestsRun,
                CopyToSeparateFolders = args.Clone,
                DirToCloneTests = args.DirToCloneTests,
                NeedOutput = !string.IsNullOrEmpty(args.OutputReport),
                NeedSynchronizationForTests = args.Sync,
                ProcessCount = args.ProcessCount,
                ResolveEventHandler = Program.LoadFromSameFolder,
                RuntimeFramework = args.RuntimeFramework,
                StartDelay = args.StartDelay,
                TestDllPath = path,
                OptimizeOrder = args.Prefetch,
                Categories = args.Include ?? args.Exclude,
                IncludeCategories = args.Include != null && args.Exclude != null
            };
        }

        private static SimpleUpdater BuildUpdater(bool labels, bool teamcity, IUpdater updater)
        {
            if(teamcity)return new TeamcityUpdater(updater);
            return labels ? 
                new NUnitLabelsUpdater(updater) : 
                new SimpleUpdater(updater);
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
