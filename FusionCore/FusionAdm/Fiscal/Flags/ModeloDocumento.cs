using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ModeloDocumento
    {
        [Description("NF-e")]
        NFe = 55,

        [Description("NFC-e")]
        NFCe = 65,

        [Description("CT-e")]
        CTe = 57,

        [Description("MDF-e")]
        MDFe = 58,

        [Description("SAT-Fiscal")]
        SAT = 59,

        [Description("CTe OS")]
        CTeOS = 67
    }
}