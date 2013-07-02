using System;

namespace Common.MT
{
	public abstract class ProgressModelSinglethreadedBase : IProgressModel
	{
		public void Start( IUpdater updater, Action<IUpdater> func, Action onEnd )
		{
			func(updater);
			onEnd();
		}

		public abstract void DoEvents();
	}
}
