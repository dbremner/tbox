using System;
using System.Runtime.InteropServices;

namespace Mnk.Library.WpfWinForms
{
	static class NativeMethods
	{
		[DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern int DestroyIcon(IntPtr hIcon);

		[DllImport("Shell32", CharSet = CharSet.Unicode)]
		public static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phIconLarge, IntPtr[] phIconSmall, int nIcons);

		[DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

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
		public static extern long SendMessageTimeout(IntPtr hwnd, int wMsg, long wParam, ref COPYDATASTRUCT lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

	}
}
