using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum CteStatus
    {
        [Description("Em Digitação")]
        EmDigitacao,
        [Description("Pendente")]
        Pendente,
        [Description("Autorizado")]
        Autorizado,
        [Description("Cancelada")]
        Cancelada,
        [Description("Inutilizado")]
        Inutilizado
    }
}