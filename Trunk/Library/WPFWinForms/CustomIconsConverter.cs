﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using WPFWinForms.Icons;

namespace WPFWinForms
{
    public class CustomIconsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Icon) value).ToImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
