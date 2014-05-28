using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.Drawings.Background
{
	public class GridBackground : SolidBackground
	{
		private readonly Pen osi;
		private readonly int cellSize;
		public GridBackground(Brush brush, Pen pen, Pen osi, int cellSize):base(brush, pen)
		{
			this.cellSize = cellSize;
			this.osi = osi;
		}

		public GridBackground() : this(Brushes.White, new Pen(Brushes.Silver,1), new Pen(Brushes.Silver,2), 20)
		{
		}

		public override void Draw(DrawingContext dc, Rect rect)
		{
			base.Draw(dc, rect);
			DrawGrid(dc, rect);
			DrawOsi(dc, rect);
		}

		private void DrawGrid(DrawingContext dc, Rect r)
		{
			var x = CalcBegin(r.Width);
			for (var i = x; i < r.Width; i += cellSize)
			{
				dc.DrawLine(Pen, new Point(i, 0), new Point(i, r.Height));
			}
			var y = CalcBegin(r.Height);
			for (var i = y; i < r.Height; i += cellSize)
			{
				dc.DrawLine(Pen, new Point(0, i), new Point(r.Width, i));
			}
		}

		private void DrawOsi(DrawingContext dc, Rect r)
		{
			var ox = r.Width/2;
			dc.DrawLine(osi, new Point(ox, 0), new Point(ox, r.Height));
			for (var i = 1; i < 4; i++)
			{
				var oy = i * r.Height / 4;
				dc.DrawLine(osi, new Point(0, oy), new Point(r.Width, oy));
			}	
		}

		private int CalcBegin(double size)
		{
			var center = (int)((size / cellSize) / 2.0);
			return (int)( size / 2 - center*cellSize);
		}
	}
}
