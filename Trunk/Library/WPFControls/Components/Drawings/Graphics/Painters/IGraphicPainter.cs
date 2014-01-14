using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WPFControls.Components.Drawings.Graphics.Painters
{
	public interface IGraphicPainter
	{
		void Paint(DrawingContext dc, Rect r, float min, float max, IList<float> values, int count, Pen pen);
		float GetValue(float position, IList<float> values, int count);
		void RecalcCache(IList<float> values, int count, Rect r, int pointsDistance);
		void ClearCache();
	}
}
