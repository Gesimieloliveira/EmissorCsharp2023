using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    public class BooleanToSnConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = value as bool? ?? false;
            return booleanValue ? "Sim" : "Não";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;

            if (stringValue == null)
                return false;

            return stringValue.ToLower() == "sim";
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}