using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    public class NullToBooleanConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            var tipo = value.GetType();

            if (tipo == typeof (string))
            {
                return string.IsNullOrEmpty((string) value);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("NullToBooleanConverter só pode ser utilizada no modo OneWay.");
        }
    }
}
