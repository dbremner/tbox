using System;
using System.Text;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    class NUnitLabelsUpdater : SimpleUpdater
    {
        public NUnitLabelsUpdater(IUpdater u, Synchronizer synchronizer) : base(u, synchronizer){}
        public override void Update(string text)
        {
        }

        protected override void ProcessResults(int allCount, Result[] items)
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
