using System;
using System.Linq;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using NUnit.Core;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    class TeamcityUpdater : GroupUpdater
    {
        private readonly object sync = new object();
        public TeamcityUpdater(IUpdater updater, int totalCount)
            : base(updater, totalCount)
        {
        }

        public override void Update(string text)
        {
        }

        protected override void ProcessResults(int allCount, Result[] items, ISynchronizer synchronizer, ITestsConfig config)
        {
            lock (sync)
            {
                foreach (var result in items.Where(x => x.IsTest))
                {
                    var name = Escape(result.FullName);
                    Console.WriteLine("##teamcity[testStarted name='{0}']", name);
                    if (!string.IsNullOrEmpty(result.Output))
                    {
                        Console.WriteLine("##teamcity[testStdOut name='{0}' out='{1}']", name, Escape(result.Output));
                    }
                    if (result.State == ResultState.Ignored || result.State == ResultState.Inconclusive || result.State == ResultState.Skipped)
                    {
                        Console.WriteLine("##teamcity[testIgnored name='{0}' message='{1}']", name, Escape(result.Message));
                    }
                    if (result.State == ResultState.Error || result.State == ResultState.Failure)
                    {
                        Console.WriteLine("##teamcity[testFailed name='{0}' message='{1}' details='{2}']", name, Escape(result.Message), Escape(result.StackTrace));
                    }
                    Console.WriteLine("##teamcity[testFinished name='{0}' duration='{1}']", name, result.Time);
                }
            }
        }

        private static string Escape(string text)
        {
            return text != null ? text
                .Replace("'", "|'")
                .Replace("\n", "|n")
                .Replace("\r", "|r")
                .Replace("\u0085", "|x")
                .Replace("\u2028", "|l")
                .Replace("\u2029", "|p")
                .Replace("|", "||") 
                .Replace("[", "|[")
                .Replace("]", "|]")
               : null;
        }
    }
}
