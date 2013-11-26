using System.Windows;
using System.Windows.Media;

namespace WPFControls.Drawings.Background
{
	public class SolidBackground : IBackground
	{
		protected Brush Brush { get; private set; }
		protected Pen Pen { get; private set; }

		public SolidBackground(Brush brush, Pen pen)
		{
			Brush = brush;
			Pen = pen;
		}

		public SolidBackground() :  this(Brushes.White, new Pen())
		{
		}

		public virtual void Draw(DrawingContext dc, Rect r)
		{
			dc.DrawRectangle(Brush, Pen, r);
		}
	}
}
