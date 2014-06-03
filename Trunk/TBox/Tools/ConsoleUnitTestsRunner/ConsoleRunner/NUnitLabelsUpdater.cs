using System;
using System.Text;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    class NUnitLabelsUpdater : SimpleUpdater
    {
        public NUnitLabelsUpdater(IUpdater updater) : base(updater){}
        public override void Update(string text)
        {
        }

        protected override void ProcessResults(int allCount, Result[] items, ISynchronizer synchronizer, ITestsConfig config)
        {
            if (items == null) return;

            var sb = new StringBuilder();
            foreach (var result in items)
            {
                sb.AppendFormat("***** {0}", result.FullName).AppendLine();
            }
            Console.Write(sb);
        }
    }
}
