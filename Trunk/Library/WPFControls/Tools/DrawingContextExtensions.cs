using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WPFControls.Tools
{
	public static class DrawingContextExtensions
	{
		public static void DrawAlignedText(this DrawingContext dc, Point p, string text)
		{
			var ft = text.CreateFormattedText();
            p.X = Math.Max(0, p.X - ft.Width / 2);
			p.Y -= ft.Height / 2;
			dc.DrawText(ft, p);
		}

		public static FormattedText CreateFormattedText(this string text)
		{
			return new FormattedText(text??string.Empty, CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				new Typeface("calibri"), 10, Brushes.DarkBlue);
		}

		public static void DrawArrow(this DrawingContext dc, Pen pen, Point from, Point to)
		{
			dc.DrawLine(pen, from, to);
			var x = to.X + ((from.X < to.X) ? -1 : 1) * 8;
			var segments = new[]
				{
					new LineSegment(new Point(x, to.Y-4), true), 
					new LineSegment(new Point(x, to.Y+4), true)
				};
			var figure = new PathFigure(to, segments, true);
			var geo = new PathGeometry(new[] { figure });
			dc.DrawGeometry(pen.Brush, null, geo);
		}

	}
}
