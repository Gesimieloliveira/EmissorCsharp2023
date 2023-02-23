using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using FusionCore.FusionAdm.Fiscal.Flags;
using static FusionCore.FusionAdm.Fiscal.Flags.TipoEmissao;

namespace FusionCore.EnumBindingFilters
{
    public class ApenasContingenciaConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (TipoEmissao[]) value;
            return source.Where(tp => tp == ContigenciaSVCAN || tp == ContigenciaSVCRS);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}