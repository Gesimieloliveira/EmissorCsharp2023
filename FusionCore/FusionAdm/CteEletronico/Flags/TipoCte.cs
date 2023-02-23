using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoCte
    {

        [Description("Normal")]
        Normal = 0,

        [Description("CT-e de Complemento de Valores")]
        ComplementoDeValores = 1,

        [Description("CT-e de Anulação")]
        CteDeAnulacao = 2,

        [Description("CT-e de Substituição")]
        CteDeSubstituicao = 3
    }
}