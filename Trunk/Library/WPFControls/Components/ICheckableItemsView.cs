using System.Collections;
using System.Windows;

namespace Mnk.Library.WpfControls.Components
{
	public interface ICheckableItemsView
	{
		event RoutedEventHandler OnCheckChanged;
		void Refresh();
		IEnumerable ItemsSource { get; }
	}
}
