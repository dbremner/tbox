using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.Drawings.Graphics
{
	class PolilynesDrawler
	{
		private readonly PathGeometry pg = new PathGeometry();
		private readonly PathFigure pf = new PathFigure();
		private readonly PolyLineSegment ps = new PolyLineSegment { IsStroked = true, IsSmoothJoin = true, Points = new PointCollection(4096)};
		private Brush brush = Brushes.Transparent;

		public PolilynesDrawler()
		{
			pf.Segments.Add(ps);
			pg.Figures.Add(pf);
		}

		public void SetBrush(Brush b)
		{
			brush = b;
		}

		public void Reset(Point startPoint)
		{
			pf.StartPoint = startPoint;
			ps.Points.Clear();
		}

		public void AddPoint(Point p)
		{
			ps.Points.Add(p);
		}

		public void Reset(double x, double y)
		{
			Reset(new Point(x, y));
		}

		public void AddPoint(double x, double y)
		{
			AddPoint(new Point(x, y));
		}

		public void Draw(DrawingContext dc, Pen pen)
		{
			dc.DrawGeometry(brush, pen, pg);
			
		}

		public int Count
		{
			get { return ps.Points.Count; }
		}

	}
}
