using System;
using System.Linq;
using System.Windows.Markup;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionWPF.Helpers
{
    public class EnumBindingSourceCsosn : MarkupExtension
    {
        private Type _enumType;

        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (value == _enumType)
                    return;

                if (null != value)
                {
                    var enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                _enumType = value;
            }
        }

        public EnumBindingSourceCsosn()
        {
        }

        public EnumBindingSourceCsosn(Type enumType) : this()
        {
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = Enum.GetValues(actualEnumType).Cast<CsosnCst>().Where(
                c => c.Equals(CsosnCst.CST102) || c.Equals(CsosnCst.CST300) || c.Equals(CsosnCst.CST500));


            return enumValues.ToArray();
        }
    }
}