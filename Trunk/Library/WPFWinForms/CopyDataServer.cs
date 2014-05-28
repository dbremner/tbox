using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Mnk.Library.WpfWinForms
{
	public sealed class CopyDataServer : NativeWindow, IDisposable
	{
		public event Action<object, string> MessageReceived;
		public CopyDataServer()
		{
			CreateHandle(new CreateParams());
		}

		protected override void WndProc(ref Message msg)
		{
			base.WndProc(ref msg);
			if (msg.Msg != NativeMethods.WM_COPYDATA) return;
			if (MessageReceived == null)return;
			MessageReceived(this, DecodeString(msg.LParam));
		}

		public static string DecodeString(IntPtr lParam)
		{
			var data = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.COPYDATASTRUCT));
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
