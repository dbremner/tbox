using System.Windows;

namespace Mnk.Library.WpfControls.Components.Units
{
    public interface ICheckableUnit : IUnit
    {
        DataTemplate ItemTemplate { get; set; }
        void OnCheckChangedEvent(object sender, RoutedEventArgs routedEventArgs);
    }
}
