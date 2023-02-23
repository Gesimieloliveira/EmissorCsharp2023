using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    public class RemoveAccentsConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RemoverAcentos(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public string RemoverAcentos(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            input = Regex.Replace(input, "[áàâãª]", "a");
            input = Regex.Replace(input, "[ÁÀÂÃÄ]", "A");
            input = Regex.Replace(input, "[éèêë]", "e");
            input = Regex.Replace(input, "[ÉÈÊË]", "E");
            input = Regex.Replace(input, "[íìîï]", "i");
            input = Regex.Replace(input, "[ÍÌÎÏ]", "I");
            input = Regex.Replace(input, "[óòôõöº]", "o");
            input = Regex.Replace(input, "[ÓÒÔÕÖ]", "O");
            input = Regex.Replace(input, "[úùûü]", "u");
            input = Regex.Replace(input, "[ÚÙÛÜ]", "U");
            input = Regex.Replace(input, "[Ç]", "C");
            input = Regex.Replace(input, "[ç]", "c");

            return input;
        }
    }
}