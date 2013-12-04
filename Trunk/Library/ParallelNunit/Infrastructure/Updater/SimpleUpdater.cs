using Common.MT;
using Common.Tools;
using ParallelNUnit.Core;
using ParallelNUnit.Execution;
using ParallelNUnit.Infrastructure.Interfaces;

namespace ParallelNUnit.Infrastructure.Updater
{
    public class SimpleUpdater : IProgressStatus
    {
        private readonly IUpdater u;
        private readonly Synchronizer synchronizer;
        private int passedCount = 0;
        private int failedCount = 0;
        private readonly object locker = new object();

        public SimpleUpdater(IUpdater u, Synchronizer synchronizer)
        {
            this.u = u;
            this.synchronizer = synchronizer;
        }

        public virtual void Update(string text)
        {
            u.Update(i => string.Format("{0}, time: {1}",text, i.FormatTimeInSec()), 0,1);
        }

        public bool UserPressClose { get { return u.UserPressClose; } }

        public void Update(int allCount, Result[] items, int failed)
        {
            lock (locker)
            {
                passedCount += items.Length;
                failedCount += failed;
            }
            ProcessResults(allCount, items);
        }

        protected virtual void ProcessResults(int allCount, Result[] items)
        {
            var caption = string.Format("Tested: {0}/{1}, failed: {2}, finished = {3}/{4}",
                                        passedCount,
                                        allCount, failedCount,
                                        synchronizer.Finished, synchronizer.Count);
            u.Update(i => string.Format("{0}, time: {1}", caption, i.FormatTimeInSec()), passedCount, allCount);
        }
    }
}
