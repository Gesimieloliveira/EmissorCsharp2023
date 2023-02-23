using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum MDFeStatus
    {
        [Description("Em Digitação")]
        EmDigitacao,
        [Description("Pendente")]
        ConsultaProcessamento,
        [Description("Autorizado")]
        Autorizado,
        [Description("Cancelada")]
        Cancelada,
        [Description("Encerrada")]
        Encerrada
    }
}