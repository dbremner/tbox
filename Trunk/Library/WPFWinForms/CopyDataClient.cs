using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mnk.Library.WpfWinForms
{
	[Serializable]
	public sealed class CopyDataClient
	{
		private readonly int windowId;
		public CopyDataClient(int windowId)
		{
			this.windowId = windowId;
		}

		public IntPtr Send(string message, int timeout = 30000)
		{
			IntPtr dummy;
			var data = CreateMessage(message);
			try
			{
				NativeMethods.SendMessageTimeout(new IntPtr(windowId),
										 NativeMethods.WM_COPYDATA,
										 (long)IntPtr.Zero,
										 ref data,
										 NativeMethods.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG,
										 (uint)timeout,
										 out dummy);
			}
			finally
			{
				Marshal.FreeCoTaskMem(data.lpData);
			}
			return dummy;
		}

		private static NativeMethods.COPYDATASTRUCT CreateMessage(string message)
		{
			var data = new NativeMethods.COPYDATASTRUCT();
			var bytes = Encoding.UTF8.GetBytes(message);
			var intPtr = Marshal.AllocCoTaskMem(bytes.Length);
			Marshal.Copy(bytes, 0, intPtr, bytes.Length);
			data.cbData = bytes.Length;
			data.dwData = IntPtr.Zero;
			data.lpData = intPtr;
			return data;
		}
	}
}
