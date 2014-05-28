using System;
using System.Drawing;
using System.Linq;

namespace Mnk.Library.WpfWinForms.Icons
{
	public sealed class IconsExtractor
	{
		public Icon GetIcon(string path, int id, bool large = false)
		{
			var hDummy = new [] {IntPtr.Zero};
			var hIconEx = new [] {IntPtr.Zero};
			try
			{
				var readIconCount = large ?
					NativeMethods.ExtractIconEx(path, id, hIconEx, hDummy, 1) :
					NativeMethods.ExtractIconEx(path, id, hDummy, hIconEx, 1);
				if (readIconCount <= 0 || hIconEx[0] == IntPtr.Zero)
					return null;
				return (Icon) Icon.FromHandle(hIconEx[0]).Clone();
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Could not extract icon", ex);
			}
			finally
			{
				foreach (var ptr in hIconEx.Where(ptr => ptr != IntPtr.Zero))
					NativeMethods.DestroyIcon(ptr);
				foreach (var ptr in hDummy.Where(ptr => ptr != IntPtr.Zero))
					NativeMethods.DestroyIcon(ptr);
			}
		}
	}
}
