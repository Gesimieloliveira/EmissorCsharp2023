using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;
using FusionLibrary.Helper.Diversos;

namespace FusionLibrary.Wpf.Conversores
{
    public class TelefoneMaskConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string) value)) return value;

            var hidratado = Regex.Replace((string) value, @"[^\d]", "");
            var apenasNumeros = StringHelper.LimitarTamanho(hidratado, 11);
            var valorBase = System.Convert.ToUInt64(apenasNumeros);
            var tamanho = apenasNumeros.Length;

            //@"(64) 8149-4233";
            //@"(64) 98149-4233";
            string[] formatos =
            {
                @"",
                @"(0",
                @"(00)",
                @"(00) 0",
                @"(00) 00",
                @"(00) 000",
                @"(00) 0000",
                @"(00) 0000-0",
                @"(00) 0000-00",
                @"(00) 0000-000",
                @"(00) 0000-0000",
                @"(00) 00000-0000"
            };

            var formato = formatos[tamanho];
            return valorBase.ToString(formato);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var apenasNumeros = Regex.Replace((string) value, @"[^\d]", "");
            return StringHelper.LimitarTamanho(apenasNumeros, 11);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}