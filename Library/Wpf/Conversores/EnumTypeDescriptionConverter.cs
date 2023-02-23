using System;
using System.ComponentModel;
using System.Globalization;

namespace FusionLibrary.Wpf.Conversores
{
    public class EnumTypeDescriptionConverter : EnumConverter
    {
        public EnumTypeDescriptionConverter(Type type) : base(type)
        {
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType != typeof (string))
                return base.ConvertTo(context, culture, value, destinationType);

            var fi = value?.GetType().GetField(value.ToString());

            if (fi == null) return string.Empty;

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

            return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description)))
                ? attributes[0].Description
                : value.ToString();
        }
    }
}