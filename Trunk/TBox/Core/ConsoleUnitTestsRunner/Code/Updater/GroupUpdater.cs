﻿using Common.MT;
using ConsoleUnitTestsRunner.Code.Interfaces;

namespace ConsoleUnitTestsRunner.Code.Updater
{
	public class GroupUpdater : IProgressStatus
	{
		private readonly IUpdater u;
		private int passedCount = 0;
		private int failedCount = 0;
		private readonly int overalCount;

		public GroupUpdater(IUpdater u, int overalCount)
		{
			this.u = u;
			this.overalCount = overalCount;
		}

		public void Update(string text)
		{
		}

		public bool UserPressClose { get { return u.UserPressClose; } }

		public void Update(int allCount, int count, int failed)
		{
			passedCount += count;
			failedCount += failed;
			var caption = string.Format("Tested: {0}/{1}, failed: {2}", passedCount, overalCount, failedCount);
			u.Update(i => string.Format("{0}, time: {1}", caption, i), passedCount, overalCount);
		}
	}
}
