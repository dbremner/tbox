using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Execution;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Updater
{
    public class GroupUpdater : IProgressStatus
    {
        private readonly IUpdater u;
        private int passedCount = 0;
        private int failedCount = 0;
        private readonly int overalCount;
        private readonly object locker = new object();

        public GroupUpdater(IUpdater u, int overalCount)
        {
            this.u = u;
            this.overalCount = overalCount;
        }

        public void Update(string text)
        {
        }

        public bool UserPressClose { get { return u.UserPressClose; } }

        public void Update(int allCount, Result[] items, int failed)
        {
            lock (locker)
            {
                passedCount += items.Length;
                failedCount += failed;
            }
            var caption = string.Format("Tested: {0}/{1}, failed: {2}", passedCount, overalCount, failedCount);
            u.Update(i => string.Format("{0}, time: {1}", caption, i.FormatTimeInSec()), passedCount, overalCount);
        }
    }
}
