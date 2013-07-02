using System.Windows;
using System.Windows.Controls;

namespace WPFControls.Components
{
	public class ExtListBox : ListBox
	{
		public ExtListBox()
		{
			AlternationCount = 2;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;
			SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
		}
	}
}
