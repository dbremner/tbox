using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WPFControls.Drawings.Background;

namespace WPFControls.Drawings.Graphics
{
	public class Graph : GraphPanel
	{
		private readonly IList<IGraphic> graphics = new List<IGraphic>();

		public Graph()
		{
			Back = new CaptionedGridBackground("x", "y");
			Counts = new int[0];
			PointsDistance = 3;
		}

		public void Clear()
		{
			Counts = new int[0];
			Min = Max = 0;
			graphics.Clear();
			Redraw();
		}

		public void Add(IGraphic gr)
		{
			Counts = new int[graphics.Count+1];
			graphics.Add(gr);
		}

		protected override void Update(Rect r)
		{
			Counts = graphics.Select(x => x.Count).ToArray();
			for (var i = 0; i < graphics.Count; ++i)
			{
				graphics[i].RecalcParams(Counts[i], r, PointsDistance);
			}
			Min = graphics.Min(x => x.Min);
			Max = graphics.Max(x => x.Max);
		}

		protected override void Render(DrawingContext dc, Rect r)
		{
			if (Counts.Min() < 2) return;
			for (var i = 0; i < graphics.Count; ++i)
			{
				graphics[i].Paint(dc, r, Min, Max, Counts[i]);
			}
		}

		public IEnumerable<float> GetValues(float position)
		{
			return Any ? graphics.Select((g,i) => g.GetValue(position, Counts[i])) : new float[0];
		}

		public int[] Counts { get; private set; }
		public float Min { get; private set; }
		public float Max { get; private set; }
		public override bool Any { get { return graphics.Count > 0 && graphics.All(x => x.Any); } }
		public float Average{get{return graphics.Average(x => x.Average);}}
		public float Last{get{return First.Last;}}
		public IGraphic First{get{return graphics.First();}}
		public int PointsDistance { get; set; }
	}
}
