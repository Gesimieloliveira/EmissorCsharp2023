using System;
using System.Globalization;
using System.Windows.Data;

namespace FusionPdv.Conversores
{
    public class IntBinaryToParamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1.Equals(value) ? parameter : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IntBinaryToParam não pode converter um valor de volta");
        }
    }
}
