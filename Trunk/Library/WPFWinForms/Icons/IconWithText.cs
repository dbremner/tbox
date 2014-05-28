using System;
using System.Drawing;
using System.Drawing.Text;

namespace Mnk.Library.WpfWinForms.Icons
{
	public sealed class IconWithText : IDisposable
	{
		public Icon Icon { get; private set; }
		private const int Size = 16;
		private readonly Bitmap bitmap = new Bitmap(Size, Size);
		private readonly Font font;
		private readonly Brush fontBrush;
		private readonly Brush backBrush;
		private readonly Graphics graphics;
		private readonly Rectangle r = new Rectangle(0, 0, Size, Size);
		private readonly StringFormat sf = new StringFormat
			                                   {
				                                   Alignment = StringAlignment.Center,
				                                   LineAlignment = StringAlignment.Center,
				                                   FormatFlags = StringFormatFlags.NoWrap,
			                                   };

		public IconWithText(string fontName = "calibri", int fontSize = Size/2+1, Color background = default(Color))
		{
			font = new Font(fontName, fontSize, FontStyle.Regular);
			fontBrush = new SolidBrush(Color.White);
			backBrush = new SolidBrush(background == default(Color) ? Color.Black : background );
			graphics = Graphics.FromImage(bitmap);
			graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
		}

		public void Create(string text)
		{
			DestroyIcon();
			graphics.FillRectangle(backBrush, r);
			graphics.DrawString(text, font, fontBrush, Size / 2, Size / 2, sf);
			var handle = bitmap.GetHicon();
			Icon = Icon.FromHandle(handle);
		}

		private void DestroyIcon()
		{
			if(Icon==null)return;
			NativeMethods.DestroyIcon(Icon.Handle);
			Icon.Dispose();
			Icon = null;
		}

		public void Dispose()
		{
			DestroyIcon();
			font.Dispose();
			fontBrush.Dispose();
			backBrush.Dispose();
			graphics.Dispose();
			bitmap.Dispose();
		}
	}
}
