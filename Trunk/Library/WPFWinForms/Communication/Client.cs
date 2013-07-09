using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WPFWinForms.Communication
{
	[Serializable]
	public sealed class Client
	{
		private readonly int windowId;
		public Client(int windowId)
		{
			this.windowId = windowId;
		}

		public IntPtr Send(string message, uint timeOut = 30000)
		{
			IntPtr dummy;
			var data = CreateMessage(message);
			try
			{
				Win32.SendMessageTimeout(new IntPtr(windowId),
										 Win32.WM_COPYDATA,
										 (int)IntPtr.Zero,
										 ref data,
										 Win32.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG,
										 timeOut,
										 out dummy);
			}
			finally
			{
				Marshal.FreeCoTaskMem(data.lpData);
			}
			return dummy;
		}

		private static Win32.COPYDATASTRUCT CreateMessage(string message)
		{
			var data = new Win32.COPYDATASTRUCT();
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
