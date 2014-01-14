using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Mnk.Library.WPFControls.Components.Drawings.Background;
using Mnk.Library.WPFControls.Components.Drawings.Graphics;
using Mnk.Library.WPFControls.Components.Drawings.Graphics.Painters;

namespace Mnk.Library.WPFControls.Components.Units
{
	/// <summary>
	/// Interaction logic for GraphUnit.xaml
	/// </summary>
	public partial class GraphUnit
	{
		private readonly CaptionedGridBackground background = new CaptionedGridBackground("x","y");
		private readonly ToolTip toolTip = new ToolTip { StaysOpen = true, Placement = PlacementMode.RelativePoint};

		public string OxCaption
		{
			get { return background.OxCaption; }
			set { background.OxCaption = value; }
		}

		public string OyCaption
		{
			get { return background.OyCaption; }
			set { background.OyCaption = value; }
		}

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public GraphUnit()
		{
			InitializeComponent();
			GraphPlot.Add(new Graphic(new Pen(Brushes.DarkGreen, 1), new PolylinesStretchGraphicPainter()));
			GraphPlot.MouseMove += GraphPlotOnMouseMove;
			GraphPlot.ToolTip = toolTip;
			GraphPlot.Back = background;
			StartTime = DateTime.MinValue;
			EndTime = DateTime.MinValue;
		}

		private void GraphPlotOnMouseMove(object sender, MouseEventArgs e)
		{
			var p = e.GetPosition(GraphPlot);
			var position = (float)(p.X / GraphPlot.ActualWidth);
			var line = string.Format("{0:0.##}%", 100 * position) + Environment.NewLine +
				string.Join(Environment.NewLine, GraphPlot.GetValues(position).Select(x => string.Format("{0:0.##}", x)));
			if (StartTime != DateTime.MinValue)
			{
				var end = EndTime;
				if (end == DateTime.MinValue) end = DateTime.Now;
				var delta = (end - StartTime).TotalSeconds * position;
				line += Environment.NewLine + StartTime.AddSeconds(delta).ToString("T");
			}
			toolTip.Content = line;
			toolTip.HorizontalOffset = p.X + 20;
			toolTip.VerticalOffset = p.Y;
		}

		public void AddGrapic(IGraphic graphic)
		{
			GraphPlot.Add(graphic);
		}

		public void Clear()
		{
			GraphPlot.Clear();
		}

		public void Redraw()
		{
			if (GraphPlot.Any)
			{
				lMin.Content = GraphPlot.Min;
				lMax.Content = GraphPlot.Max;
				lAvg.Content = GraphPlot.Average;
				lCurrent.Content = GraphPlot.Last;
			}
			else
			{
				lMin.Content = 0;
				lMax.Content = 0;
				lAvg.Content = 0;
				lCurrent.Content = 0;
			}
			GraphPlot.Redraw();
		}

		public Graph Graph { get { return GraphPlot; } }
		public IGraphic MainGraphic { get { return GraphPlot.First; } }

		private void EdgeModeChanged(object sender, RoutedEventArgs e)
		{
			RenderOptions.SetEdgeMode(this, 
				(Aliased.IsChecked == true) ? EdgeMode.Aliased : EdgeMode.Unspecified);
			if (Graph != null)
			{
				Graph.PointsDistance = (Aliased.IsChecked == true) ? 3 : 2;
				Redraw();
			}
		}
	}
}
