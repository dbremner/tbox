using System;
using System.Text;
using Common.MT;
using ParallelNUnit.Core;
using ParallelNUnit.Infrastructure;
using ParallelNUnit.Infrastructure.Updater;

namespace ConsoleUnitTestsRunner.ConsoleRunner
{
    class NUnitLabelsUpdater : SimpleUpdater
    {
        public NUnitLabelsUpdater(IUpdater u, Synchronizer synchronizer) : base(u, synchronizer){}
        public override void Update(string text)
        {
        }

        protected override void ProcessResults(int allCount, Result[] items)
        {
            var sb = new StringBuilder();
            foreach (var result in items)
            {
                sb.AppendFormat("***** {0}", result.FullName).AppendLine();
            }
            Console.Write(sb);
        }
    }
}
