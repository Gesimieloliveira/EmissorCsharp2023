using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    public class CpfCnpjMaskConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string) value)) return value;
            var documento = (string) value;
            var apenasNumeros = Regex.Replace((string) value, @"[^\d]", "");

            if (apenasNumeros.Length == 11)
            {
                var cpfConverter = new CpfMaskConverter();
                return cpfConverter.Convert(apenasNumeros, targetType, parameter, culture);
            }

            if (apenasNumeros.Length == 14)
            {
                var cnpjConverter = new CnpjMaskConverter();
                return cnpjConverter.Convert(apenasNumeros, targetType, parameter, culture);
            }

            return documento;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var apenasNumeros = Regex.Replace((string) value, @"[^\d]", "");
            return apenasNumeros;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}