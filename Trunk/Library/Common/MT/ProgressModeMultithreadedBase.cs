using System;
using System.Threading;

namespace Common.MT
{
	public abstract class ProgressModeMultithreadedBase : IProgressModel
	{
		protected static void Work( object obj )
		{
			var x = (ProgressModelData)obj;
			x.Func(x.Updater);
			x.OnEnd();
		}

		public void Start( IUpdater updater, Action<IUpdater> func, Action onEnd )
		{
			var thread = new Thread( Work );
			//thread.SetApartmentState(ApartmentState.STA);
			thread.Start(
				new ProgressModelData
					{
						Updater = updater, 
						Func = func, 
						OnEnd = onEnd
					});
		}

		public abstract void DoEvents();
	}
}
