using System;
using System.Runtime.InteropServices;

namespace WPFWinForms.Communication
{
	static class Win32
	{
		public struct COPYDATASTRUCT
		{
			public IntPtr dwData;
			public int cbData;
			public IntPtr lpData;
		}
		[Flags]
		public enum SendMessageTimeoutFlags : uint
		{
			SMTO_NORMAL = 0u,
			SMTO_BLOCK = 1u,
			SMTO_ABORTIFHUNG = 2u,
			SMTO_NOTIMEOUTIFNOTHUNG = 8u
		}
		public const int WM_COPYDATA = 74;
		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern int SendMessageTimeout(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);
	}
}
