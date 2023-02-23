using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum TipoEmissao
    {
        [Description("Normal")]
        Normal = 1,

        [Description("EPEC")]
        ContigenciaEPEC = 4,

        [Description("Servidor Virtual Contingência AN")]
        ContigenciaSVCAN = 6,

        [Description("Servidor Virtual Contigência RS")]
        ContigenciaSVCRS = 7,

        [Description("Contigência Offline")]
        ContigenciaOfflineNFCe = 9
    }
}