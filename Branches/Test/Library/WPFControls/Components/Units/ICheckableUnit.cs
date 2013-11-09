using System.Windows;

namespace WPFControls.Components.Units
{
    public interface ICheckableUnit : IUnit
    {
        DataTemplate ItemTemplate { get; set; }
        void OnCheckChangedEvent(object sender, RoutedEventArgs routedEventArgs);
    }
}
