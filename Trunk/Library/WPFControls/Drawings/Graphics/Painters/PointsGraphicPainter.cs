using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WPFControls.Drawings.Graphics.Painters
{
	public class PointsGraphicPainter : IGraphicPainter
	{
		private readonly PolilynesDrawler drawler = new PolilynesDrawler();
		public void Paint(DrawingContext dc, Rect r, float min, float max, IList<float> values, int count, Pen pen)
		{
			var scale = r.Height / (max - min);
			drawler.Reset(CalcPoint(0, values[0], scale, r, min));
			for (var i = 1; i < count; i++)
			{
				drawler.AddPoint(CalcPoint(i, values[i], scale, r, min));
			}
			drawler.Draw(dc, pen);
		}

		public float GetValue(float position, IList<float> values, int count)
		{
			return 0;
		}

		public void RecalcCache(IList<float> values, int count, Rect r, int pointsDistance)
		{
			
		}

		public void ClearCache()
		{
		}

		private static Point CalcPoint(float x, float y, double scale, Rect r, float min)
		{
			return new Point(
					r.X + r.Width - x,
					(int)(r.Y + r.Height - scale * (y - min))
				);
		}
	}
}
