using System;
using System.Globalization;
using System.Windows.Data;

namespace FusionLibrary.Wpf.Conversores
{
    public class ParamToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ParamToBoolean não pode converter um valor de volta");
        }
    }
}
