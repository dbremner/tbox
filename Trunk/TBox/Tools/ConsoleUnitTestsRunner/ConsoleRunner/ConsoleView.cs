﻿using System;
using System.Collections.Generic;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    internal class ConsoleView : IUnitTestsView
    {
        public double Time { get; set; }
        public TestsMetricsCalculator Tmc { get; private set; }
        private IList<Result> testsResults;

        public void SetItems(IList<Result> items, TestsMetricsCalculator metrics)
        {
            Tmc = metrics;
            Console.WriteLine();
            testsResults = items;
            Console.WriteLine(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds\n  Not run: {5}, Invalid: {6}, Ignored: {7}, Skipped: {8}",
                Tmc.Passed,
                Tmc.Errors,
                Tmc.Failures,
                Tmc.Inconclusive,
                Time,
                Tmc.NotRun.Length,
                Tmc.Invalid,
                Tmc.Ignored,
                Tmc.Skipped
                );

            PrintArray("Errors and Failures:", Tmc.Failed, true);
            PrintArray("Tests Not Run:", Tmc.NotRun, false);

            Console.WriteLine();
        }

        private static void PrintArray(string message, Result[] items, bool stackTrace)
        {
            if (items.Length == 0) return;
            Console.WriteLine();
            Console.WriteLine(message);
            var i = 0;
            foreach (var r in items)
            {
                Console.WriteLine("{0}) {1} : {2}{3}{4}", ++i, r.State, r.FullName,
                string.IsNullOrEmpty(r.Message) ? string.Empty : ("\n   " + r.Message),
                stackTrace ? ("\n" + r.StackTrace + "\n") : string.Empty
                );
            }
            Console.WriteLine();
        }

        public void Clear()
        {
        }

        public void GenerateReport(string path, string xmlReport)
        {
            new ReportBuilder().GenerateReport(path,xmlReport,Tmc,testsResults);
        }
    }
}
