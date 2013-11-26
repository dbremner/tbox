using System;
using Common.MT;
using Common.Tools;
using PluginsShared.UnitTests.Interfaces;

namespace PluginsShared.UnitTests.Updater
{
	public class SimpleUpdater : IProgressStatus
	{
		private readonly IUpdater u;
	    private readonly Synchronizer synchronizer;
	    private int passedCount = 0;
		private int failedCount = 0;
		private int lastTime = Environment.TickCount;

		public SimpleUpdater(IUpdater u, Synchronizer synchronizer)
		{
		    this.u = u;
		    this.synchronizer = synchronizer;
		}

	    public void Update(string text)
		{
			u.Update(text, 0);
		}

		public bool UserPressClose { get { return u.UserPressClose; } }

		public void Update(int allCount, int count, int failed)
		{
			if ((Environment.TickCount - lastTime) < 1000) return;
			lastTime = Environment.TickCount;
			passedCount += count;
			failedCount += failed;
            var caption = string.Format("Tested: {0}/{1}, failed: {2}, finished = {3}/{4}", 
                passedCount, 
                allCount, failedCount,
                synchronizer.Finished, synchronizer.Count);
			u.Update(i => string.Format("{0}, time: {1}", caption, i.FormatTimeInSec()), passedCount, allCount);
		}
	}
}
