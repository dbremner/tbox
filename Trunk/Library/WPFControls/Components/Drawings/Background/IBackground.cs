using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.Drawings.Background
{
	public interface IBackground
	{
		void Draw(DrawingContext dc, Rect rect);
	}
}
