using System.Windows.Media;
using LeaksInterceptor.Code.Getters;
using WPFControls.Drawings.Graphics;
using WPFControls.Drawings.Graphics.Painters;

namespace LeaksInterceptor.Code.Standart
{
	class CounterInfo
	{
		public IGraphic Graphic { get; private set; }
		public IGetter Getter { get; private set; }

		public CounterInfo(IGetter getter)
		{
			Getter = getter;
			Graphic = new Graphic(new Pen(Brushes.DarkBlue, 1), new PolylinesStretchGraphicPainter() );
		}
	}
}
