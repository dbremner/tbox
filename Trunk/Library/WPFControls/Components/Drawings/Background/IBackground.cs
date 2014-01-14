using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WPFControls.Components.Drawings.Background
{
	public interface IBackground
	{
		void Draw(DrawingContext dc, Rect r);
	}
}
