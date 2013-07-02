using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace WPFWinForms.Icons
{
	public sealed class IconsExtractor
	{
		[DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
		private static extern int DestroyIcon(IntPtr hIcon);

		[DllImport("Shell32", CharSet = CharSet.Auto)]
		private static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phIconLarge, IntPtr[] phIconSmall, int nIcons);

		public Icon GetIcon(string path, int id, bool large = false)
		{
			var hDummy = new [] {IntPtr.Zero};
			var hIconEx = new [] {IntPtr.Zero};
			try
			{
				var readIconCount = large ?
					ExtractIconEx(path, id, hIconEx, hDummy, 1) :
					ExtractIconEx(path, id, hDummy, hIconEx, 1);
				if (readIconCount <= 0 || hIconEx[0] == IntPtr.Zero)
					return null;
				return (Icon) Icon.FromHandle(hIconEx[0]).Clone();
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Could not extract icon", ex);
			}
			finally
			{
				foreach (var ptr in hIconEx.Where(ptr => ptr != IntPtr.Zero))
					DestroyIcon(ptr);
				foreach (var ptr in hDummy.Where(ptr => ptr != IntPtr.Zero))
					DestroyIcon(ptr);
			}
		}
	}
}
