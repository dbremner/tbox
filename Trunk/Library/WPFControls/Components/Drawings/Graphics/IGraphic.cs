using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.Drawings.Graphics
{
	public interface IGraphic
	{
		void Paint(DrawingContext dc, Rect rect, float min, float max, int count);
		int Count { get; }
		float Min { get; }
		float Max { get; }
		float Average { get; }
		float Last { get; }
		bool Any { get; }
		IEnumerable<float> Values { get; }
		void Clear();
		void Add(float value);
		float GetValue(float position, int count);
		void CalcParameters(int count, Rect rect, int pointsDistance);
	}
}
