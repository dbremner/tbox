using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WPFWinForms.Communication
{
	public sealed class Server : NativeWindow, IDisposable
	{
		public event Action<object, string> MessageReceived;
		public Server()
		{
			CreateHandle(new CreateParams());
		}

		protected override void WndProc(ref Message msg)
		{
			base.WndProc(ref msg);
			if (msg.Msg != Win32.WM_COPYDATA) return;
			if (MessageReceived == null)return;
			MessageReceived(this, DecodeString(msg.LParam));
		}

		public static string DecodeString(IntPtr lParam)
		{
			var data = (Win32.COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(Win32.COPYDATASTRUCT));
			var bytes = new byte[data.cbData];
			Marshal.Copy(data.lpData, bytes, 0, data.cbData);
			return Encoding.UTF8.GetString(bytes);
		}

		public void Dispose()
		{
			DestroyHandle();
		}
	}
}
