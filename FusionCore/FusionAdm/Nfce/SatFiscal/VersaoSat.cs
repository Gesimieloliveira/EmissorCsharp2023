using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Nfce.SatFiscal
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum VersaoSat
    {
        [Description("0.06")]
        V6 = 6,
        [Description("0.07")]
        V7 = 7,
        [Description("0.08")]
        V8 = 8,
        // todo [Description("0.09")]
        // todo V9 = 9,
    }
}