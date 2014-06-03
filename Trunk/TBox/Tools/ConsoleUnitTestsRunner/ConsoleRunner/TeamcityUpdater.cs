using System;
using System.Linq;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using NUnit.Core;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    class TeamcityUpdater : SimpleUpdater
    {
        private int passed = 0;
        private int ignored = 0;
        private int failed = 0;
        private readonly object locker = new object();
        public TeamcityUpdater(IUpdater updater) : base(updater)
        {
        }

        public override void Update(string text)
        {
        }

        protected override void ProcessResults(int allCount, Result[] items, ISynchronizer synchronizer, ITestsConfig config)
        {
            lock (locker)
            {
                foreach (var result in items.Where(x=>x.IsTest))
                {
                    switch (result.State)
                    {
                        case ResultState.Error:
                        case ResultState.Failure:
                            ++failed;
                            break;
                        case ResultState.Success:
                            ++passed;
                            break;
                        default:
                            ++ignored;
                            break;
                    }
                }
            }
            Console.Write("##teamcity[progressMessage 'Tests: {0}", passed);
            if (failed > 0) Console.Write(", failed: {0}", failed);
            if (ignored > 0) Console.Write(", ignored: {0}", ignored);
            Console.WriteLine("']");
        }
    }
}
