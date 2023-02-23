using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoDocumento
    {
        [Description("Declaração")]
        Declaracao = 0,
        [Description("Dutoviário")]
        Dutoviario = 10,
        [Description("CF-e SAT")]
        CfeSat = 59,
        [Description("NFC-e")]
        Nfce = 65,
        [Description("Outros")]
        Outros = 99
    }
}