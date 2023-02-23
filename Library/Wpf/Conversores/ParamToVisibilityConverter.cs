using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FusionLibrary.Wpf.Conversores
{
    public class ParamToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ParamToVisibility não pode converter um valor de volta");
        }
    }
}