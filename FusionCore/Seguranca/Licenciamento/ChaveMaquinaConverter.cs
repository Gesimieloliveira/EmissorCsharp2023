using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionCore.Seguranca.Licenciamento
{
    public class ChaveMaquinaConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (string) value;

            if (input == null)
                return null;

            if (input.Length != 23)
                return input;

            var regex = new Regex(@"(\w{3})(\w{5})(\w{5})(\w{5})(\w{5})");
            return regex.Replace(input, @"$1-$2-$3-$4-$5");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (string)value;
            return input?.Replace("-", "");
        }
    }
}