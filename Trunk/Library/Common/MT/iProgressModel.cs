using System;

namespace Mnk.Library.Common.MT
{
	public interface IProgressModel
	{
		void Start( IUpdater updater, Action<IUpdater> func, Action onEnd );
		void DoEvents();
	}
}