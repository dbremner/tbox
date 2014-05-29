using System;
using System.Windows.Threading;

namespace Mnk.Library.WpfControls
{
	public class DispatcherHelper
	{
		private static readonly DispatcherOperationCallback ExitFrameCallback = ExitFrame;
		public static void DoEvents()
		{
			var nestedFrame = new DispatcherFrame();
			var exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(
				DispatcherPriority.Background, ExitFrameCallback, nestedFrame );
			Dispatcher.PushFrame( nestedFrame );
			if ( exitOperation.Status != DispatcherOperationStatus.Completed )
			{
				exitOperation.Abort();
			}
		}

		private static Object ExitFrame( Object state )
		{
			var frame = state as DispatcherFrame;
			if(frame==null)
			{
				throw new ArgumentNullException("frame", "Dispatcher error!");
			}
			frame.Continue = false;
			return null;
		}
	}
}
