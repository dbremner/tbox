using Common.MT;
using WPFControls.Code.OS;

namespace WPFControls.Components.Updater
{
	public class ProgressModelSinglethreaded : ProgressModelSinglethreadedBase
	{
		public override void DoEvents()
		{
			DispatcherHelper.DoEvents();
		}
	}
}
