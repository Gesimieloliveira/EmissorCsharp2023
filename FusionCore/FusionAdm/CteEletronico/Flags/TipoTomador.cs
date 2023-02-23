using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoTomador
    {
        [Description("Remetente")]
        Remetente,
        [Description("Expedidor")]
        Expedidor,
        [Description("Recebedor")]
        Recebedor,
        [Description("Destinatário")]
        Destinatario,
        [Description("Outros")]
        Outros
    }
}