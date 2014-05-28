using System.Windows;
using System.Windows.Media;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Components.Drawings.Background
{
	public class CaptionedGridBackground : GridBackground
	{
		public string OxCaption { get; set; }
		public string OyCaption { get; set; }

		public CaptionedGridBackground(string ox = "", string oy = "")
		{
			OxCaption = ox;
			OyCaption = oy;
		}

		public override void Draw(DrawingContext dc, Rect rect)
		{
			base.Draw(dc, rect);
			DrawHorizontalFormattedText(dc, OxCaption, new Point(rect.Width / 2, rect.Height - 5));
			DrawVerticalFormattedText(dc, OyCaption, new Point(0, rect.Height/2));
		}

		private static void DrawHorizontalFormattedText(DrawingContext dc, string text, Point p)
		{
			dc.DrawAlignedText(p, text);
		}

		private static void DrawVerticalFormattedText(DrawingContext dc, string text, Point p)
		{
			var ft = text.CreateFormattedText();
			p.Y += ft.Width / 2;
			dc.PushTransform(new RotateTransform(270){CenterX = p.X, CenterY = p.Y});
			dc.DrawText(ft, p);
			dc.Pop();
		}
	}
}
