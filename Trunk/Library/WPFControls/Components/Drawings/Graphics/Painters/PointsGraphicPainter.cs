using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.Drawings.Graphics.Painters
{
	public class PointsGraphicPainter : IGraphicPainter
	{
		private readonly PolilynesDrawler drawler = new PolilynesDrawler();
		public void Paint(DrawingContext dc, Rect rect, float min, float max, IList<float> values, int count, Pen pen)
		{
			var scale = rect.Height / (max - min);
			drawler.Reset(CalcPoint(0, values[0], scale, rect, min));
			for (var i = 1; i < count; i++)
			{
				drawler.AddPoint(CalcPoint(i, values[i], scale, rect, min));
			}
			drawler.Draw(dc, pen);
		}

		public float GetValue(float position, IList<float> values, int count)
		{
			return 0;
		}

		public void RecalcCache(IList<float> values, int count, Rect rect, int pointsDistance)
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
