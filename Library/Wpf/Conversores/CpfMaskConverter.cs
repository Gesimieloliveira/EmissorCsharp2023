using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    public class CpfMaskConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string) value)) return value;

            var apenasNumeros = Regex.Replace((string) value, @"[^\d]", "");
            var valorBase = System.Convert.ToUInt64(apenasNumeros);
            var tamanho = apenasNumeros.Length;

            //@"000\.000\.000\-00";
            string[] formatos =
            {
                @"",
                @"0",
                @"00",
                @"000",
                @"000\.0",
                @"000\.00",
                @"000\.000",
                @"000\.000\.0",
                @"000\.000\.00",
                @"000\.000\.000",
                @"000\.000\.000-0",
                @"000\.000\.000-00"
            };

            var formato = formatos[tamanho];
            return valorBase.ToString(formato);
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