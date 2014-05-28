using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Mnk.Library.WpfControls.Components.Drawings.Graphics.Painters;

namespace Mnk.Library.WpfControls.Components.Drawings.Graphics
{
	public sealed class Graphic : IGraphic
	{
		private readonly Pen pen;
		private readonly IGraphicPainter painter;
		private IList<float> values;
		private double summ;
		private int lastRecalculatedCount;

		public Graphic(Pen pen, IGraphicPainter painter)
		{
			this.pen = pen;
			this.painter = painter;
			Clear();
		}

		public void Clear()
		{
			lock (painter)
			{
				values = new List<float>();
				Min = Max = Average = Last = 0;
				lastRecalculatedCount = -1;
				summ = 0;
				painter.ClearCache();
			}
		}

		public void Add(float value)
		{
			lock (painter)
			{
				values.Add(value);
			}
		}

		public float GetValue(float position, int count)
		{
			var list = values;
			if (list.Count < count) return 0;
			return painter.GetValue(position, list, count);
		}

		public void Paint(DrawingContext dc, Rect rect, float min, float max, int count)
		{
			var list = values;
			if (list.Count < count) return;
			painter.Paint(dc, rect, Math.Min(min, Min), Math.Max(max, Max), list, count, pen);
		}

		public void CalcParameters(int count, Rect rect, int pointsDistance)
		{
			var list = values;
			if (list.Count < count) return;
			painter.RecalcCache(values, count, rect, pointsDistance);
			if (lastRecalculatedCount == -1)
			{
				summ = Last = Min = Max = Average = list[0];
				lastRecalculatedCount += 2;
			}
			double s = 0;
			for (; lastRecalculatedCount < count; ++lastRecalculatedCount)
			{
				var value = list[lastRecalculatedCount];
				if (value > Max) Max = value;
				else if (value < Min) Min = value;
				s += value;
			}
			summ += s;
			Last = list.Last();
			Average = (float)(summ / count);
		}

		public int Count { get { return values.Count; } }
		public bool Any { get { return Count > 0; } }
		public float Min { get; private set; }
		public float Max { get; private set; }
		public float Average{ get; private set; }
		public float Last { get; private set; }
		public IEnumerable<float> Values { get { return values; } }

	}
}
