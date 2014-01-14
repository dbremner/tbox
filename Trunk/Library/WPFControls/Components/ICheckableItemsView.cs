using System.Collections;
using System.Windows;

namespace Mnk.Library.WPFControls.Components
{
	public interface ICheckableItemsView
	{
		event RoutedEventHandler OnCheckChanged;
		void Refresh();
		IEnumerable ItemsSource { get; }
	}
}
