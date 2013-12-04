using System;
using System.Linq;
using System.Text;
using Common.MT;
using NUnit.Core;
using ParallelNUnit.Core;
using ParallelNUnit.Infrastructure;
using ParallelNUnit.Infrastructure.Updater;

namespace ConsoleUnitTestsRunner.ConsoleRunner
{
    class TeamcityUpdater : SimpleUpdater
    {
        private int passed = 0;
        private int ignored = 0;
        private int failed = 0;
        private readonly object locker = new object();
        public TeamcityUpdater(IUpdater u, Synchronizer synchronizer) : base(u, synchronizer)
        {
        }

        public override void Update(string text)
        {
        }

        protected override void ProcessResults(int allCount, Result[] items)
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
