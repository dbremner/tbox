using System.Diagnostics;
using System.Windows.Media;
using WPFControls.Drawings.Graphics;
using WPFControls.Drawings.Graphics.Painters;

namespace LeaksInterceptor.Code.Perfomance
{
	class PerfomanceInfo
	{
		public IGraphic Graphic { get; private set; }
		public PerformanceCounter Getter { get; private set; }

		public PerfomanceInfo(PerformanceCounter getter)
		{
			Getter = getter;
			Graphic = new Graphic(new Pen(Brushes.DarkGreen, 1), new PolylinesStretchGraphicPainter() );
		}
	}
}
