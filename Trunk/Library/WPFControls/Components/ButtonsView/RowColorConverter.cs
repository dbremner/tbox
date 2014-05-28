using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.ButtonsView
{
    public class RowColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var index = (int)value;
            return index == 0 ? Brushes.Transparent : Brushes.Lavender;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
