using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;
using FusionLibrary.Helper.Diversos;

namespace FusionLibrary.Wpf.Conversores
{
    public class CnpjMaskConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string) value)) return value;

            var hidratado = Regex.Replace((string) value, @"[^\d]", "");
            var apenasNumeros = hidratado.LimitarTamanho(14);
            var valorBase = System.Convert.ToUInt64(apenasNumeros);

            //@"00\.000\.000\/0000\-00";
            string[] formatos =
            {
                @"",
                @"0",
                @"00",
                @"00\.0",
                @"00\.00",
                @"00\.000",
                @"00\.000\.0",
                @"00\.000\.00",
                @"00\.000\.000",
                @"00\.000\.000\/0",
                @"00\.000\.000\/00",
                @"00\.000\.000\/000",
                @"00\.000\.000\/0000",
                @"00\.000\.000\/0000\-0",
                @"00\.000\.000\/0000\-00"
            };

            var formato = formatos[apenasNumeros.Length];
            return valorBase.ToString(formato);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var apenasNumeros = Regex.Replace((string) value, @"[^\d]", "");
            return apenasNumeros.LimitarTamanho(14);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}