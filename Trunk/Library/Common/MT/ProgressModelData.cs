using System;

namespace Common.MT
{
	internal struct ProgressModelData
	{
		public IUpdater Updater;
		public Action<IUpdater> Func;
		public Action OnEnd;
	}
}
