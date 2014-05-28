using System.Globalization;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Execution;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Updater
{
    public class GroupUpdater : IProgressStatus
    {
        private readonly IUpdater updater;
        private int passedCount = 0;
        private int failedCount = 0;
        private readonly int overallCount;
        private readonly object locker = new object();

        public GroupUpdater(IUpdater updater, int overallCount)
        {
            this.updater = updater;
            this.overallCount = overallCount;
        }

        public void Update(string text)
        {
        }

        public bool UserPressClose { get { return updater.UserPressClose; } }

        public void Update(int allCount, Result[] items, int failed)
        {
            lock (locker)
            {
                passedCount += items.Length;
                failedCount += failed;
            }
            var caption = string.Format(CultureInfo.InvariantCulture, "Tested: {0}/{1}, failed: {2}", passedCount, overallCount, failedCount);
            updater.Update(i => string.Format(CultureInfo.InvariantCulture, "{0}, time: {1}", caption, i.FormatTimeInSec()), passedCount, overallCount);
        }
    }
}
