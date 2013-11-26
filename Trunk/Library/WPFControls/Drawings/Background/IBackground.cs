using System.Windows;
using System.Windows.Media;

namespace WPFControls.Drawings.Background
{
	public interface IBackground
	{
		void Draw(DrawingContext dc, Rect r);
	}
}
