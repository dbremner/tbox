using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.Drawings.Graphics.Painters
{
	public class PolylinesStretchGraphicPainter : IGraphicPainter
	{
		private int lastPosition = -1;
		private int lastIdent = -1;
		private int lastTargetCount = -1;
		private int ident = -1;
		private float[] minValues = new float[0];
		private float[] maxValues = new float[0];

		private readonly PolilynesDrawler drawler = new PolilynesDrawler();
		public void Paint(DrawingContext dc, Rect rect, float min, float max, IList<float> values, int count, Pen pen)
		{
			var scaleY = rect.Height / (max - min);
			var scaleX = rect.Width / (count-1);
			drawler.Reset(CalcPoint(0, values[0], scaleX, scaleY, rect, min));
			if (ident == 1)
			{
				DrawSimple(values, count, scaleX, scaleY, rect, min);
			}
			else
			{
				drawler.SetBrush(pen.Brush);
				DrawComplicated(count, scaleX, scaleY, rect, min);
			}
			drawler.Draw(dc, pen);
		}

		private static int CalcIdent(Rect r, int count, int pointsDistance)
		{
			return Math.Max(1, (int)(pointsDistance * count / r.Width));
		}

		private void DrawSimple(IList<float> values, int count, double scaleX, double scaleY, Rect r, float min)
		{
			var i = 1;
			for (; i < count; ++i)
			{
				drawler.AddPoint(CalcPoint(i, values[i], scaleX, scaleY, r, min));
			}
		}

		private void DrawComplicated(int count, double scaleX, double scaleY, Rect r, float min)
		{
			var targetCount = count / ident;
			var i = 0;
			var j = 0;
			for (; j < targetCount-1; i += ident, j++)
			{
				drawler.AddPoint(CalcPoint(i, maxValues[j], scaleX, scaleY, r, min));
			}
			drawler.AddPoint(CalcPoint(i, maxValues[j], scaleX, scaleY, r, min));
			for (; j > 0; i -= ident, j--)
			{
				drawler.AddPoint(CalcPoint(i, minValues[j], scaleX, scaleY, r, min));
			}
		}

		public void RecalcCache(IList<float> values, int count, Rect rect, int pointsDistance)
		{
			ident = CalcIdent(rect, count, pointsDistance);
			if (ident == 1)
			{
				if (lastIdent != -1) ClearCache();
			}
			else
			{
				var targetCount = count / ident;
				if (targetCount != lastTargetCount || ident != lastIdent)
					RebuildData(values, count, targetCount);
				else if (lastPosition < count) 
					ClarifyValues(values, count, targetCount);
				lastTargetCount = targetCount;
			}
			lastIdent = ident;
		}

		public void ClearCache()
		{
			lastTargetCount = -1;
			lastPosition = -1;
			lastIdent = -1;
			drawler.SetBrush(Brushes.Transparent);
		}

		public float GetValue(float position, IList<float> values, int count)
		{
			return values[(int)((1 - position) * (count - 1) + 0.5f)];
		}


		private void ClarifyValues(IList<float> values, int count, int targetCount)
		{
			var minS = minValues[lastTargetCount - 1];
			var maxS = maxValues[lastTargetCount - 1];
			for (; lastPosition < count; ++lastPosition)
			{
				var s = values[lastPosition];
				if (s > maxS) maxS = s;
				else if (s < minS) minS = s;
			}
			minValues[targetCount - 1] = minS;
			maxValues[targetCount - 1] = maxS;
		}

		private void RebuildData(IList<float> values, int count, int targetCount)
		{
			while (minValues.Length <= targetCount)
			{
				minValues = new float[2*targetCount];
				maxValues = new float[minValues.Length];
			}
			var k = 0;
			for (var i = 0; i < count; ++k)
			{
				var j = i;
				var minS = values[i];
				var maxS = minS;
				++i;
				for (; i < j + ident && i < count; ++i)
				{
					var s = values[i];
					if (s > maxS) maxS = s;
					else if (s < minS) minS = s;
				}
				minValues[k] = minS;
				maxValues[k] = maxS;
			}
			lastPosition = count;
		}

		private static Point CalcPoint(double x, double y, double scaleX, double scaleY, Rect r, double min)
		{
			return new Point(r.Width - scaleX * x, r.Height - scaleY * (y - min) );
		}
	}
}
