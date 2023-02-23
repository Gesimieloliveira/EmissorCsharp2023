using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    [ValueConversion(typeof (bool), typeof (bool))]
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var ret = value is bool b && b;

            return !ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}