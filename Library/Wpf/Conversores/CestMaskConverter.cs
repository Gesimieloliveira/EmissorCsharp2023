using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using FusionLibrary.Helper.Diversos;

namespace FusionLibrary.Wpf.Conversores
{
    public class CestMaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value)) return value;

            var hidratado = Regex.Replace((string)value, @"[^\d]", "");
            var apenasNumeros = StringHelper.LimitarTamanho(hidratado, 8);
            var valorBase = System.Convert.ToUInt64(apenasNumeros);
            var tamanho = apenasNumeros.Length;

            //@"11\.111\-11";
            string[] formatos =
            {
                @"",
                @"0",
                @"00",
                @"00\.0",
                @"00\.00",
                @"00\.000",
                @"00\.000\.0",
                @"00\.000\.00"
            };

            var formato = formatos[tamanho];
            return valorBase.ToString(formato);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var apenasNumeros = Regex.Replace((string)value, @"[^\d]", "");
            return StringHelper.LimitarTamanho(apenasNumeros, 7);
        }
    }
}
