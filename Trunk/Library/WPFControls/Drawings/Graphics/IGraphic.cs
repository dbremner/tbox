using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WPFControls.Drawings.Graphics
{
	public interface IGraphic
	{
		void Paint(DrawingContext gr, Rect r, float min, float max, int count);
		int Count { get; }
		float Min { get; }
		float Max { get; }
		float Average { get; }
		float Last { get; }
		bool Any { get; }
		IEnumerable<float> Values { get; }
		void Clear();
		void Add(float f);
		float GetValue(float position, int count);
		void RecalcParams(int count, Rect r, int pointsDistance);
	}
}
