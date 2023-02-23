using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FusionLibrary.Wpf.Conversores
{
    public class EnumDescriptionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var myEnum = (Enum) value;
            var description = GetDescription(myEnum);

            return description;
        }

        private static string GetDescription(Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length <= 0)
                return string.Empty;

            var attrs = memInfo[0].GetCustomAttributes(typeof (DescriptionAttribute), false);

            return attrs.Length > 0
                ? ((DescriptionAttribute) attrs[0]).Description
                : en.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}