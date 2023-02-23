using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FusionPdv.Conversores
{
    public class IntBinaryToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1.Equals(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Visible.Equals(value) ? 1 : 0;
        }
    }
}