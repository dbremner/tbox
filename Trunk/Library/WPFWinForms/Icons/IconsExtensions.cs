using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFWinForms.Icons
{
	public static class IconsExtensions
	{

		public static ImageSource ToImageSource(this Icon icon)
		{
			using (var bitmap = icon.ToBitmap())
			{
				var hBitmap = bitmap.GetHbitmap();

				ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
					hBitmap,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

				if (!NativeMethods.DeleteObject(hBitmap))
				{
					throw new Win32Exception();
				}

				return wpfBitmap;
			}
		}
		/*
		public static Icon ToIcon(this ImageSource source)
		{
			if (source == null) return null;
			using (var ms = new MemoryStream())
			{
				var encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
				encoder.Save(ms);
				ms.Flush();
				return new Icon(ms);
			}
		}
		*/
	}
}
