using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mnk.Library.WpfWinForms.Icons
{
	public static class IconsExtensions
	{
        private readonly static Dictionary<Icon,ImageSource> Icons = new Dictionary<Icon, ImageSource>();
		public static ImageSource ToImageSource(this Icon icon)
		{
            lock (Icons)
            {
                ImageSource value;
                if (Icons.TryGetValue(icon, out value))
                {
                    return value;
                }
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
                    Icons[icon] = wpfBitmap;
                    return wpfBitmap;
                }
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
