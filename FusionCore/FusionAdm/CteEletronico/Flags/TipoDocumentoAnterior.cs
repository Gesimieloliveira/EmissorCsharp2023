using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoDocumentoAnterior
    {
        [Description("07 - ATRE")]
        ATRE = 07,

        [Description("08 - DTA (Despacho de Transito Aduaneiro)")]
        DTA = 08,

        [Description("09 - Conhecimento Aéreo Internacional")]
        ConhecimentoAereoInternacional = 09,

        [Description("10 - Conhecimento - Carta de Porte Internacional")]
        CartaDePorteInternacional = 10,

        [Description("11 - Conhecimento Avulso")]
        ConhecimentoAvulso = 11,

        [Description("12 - TIF (Transporte Internacional Ferroviário)")]
        TIF = 12,

        [Description("13 - BL (Bil of Landing)")]
        BL = 13,

        [Description("CT-e (Conhecimento de Transporte Eletrônico)")]
        CTe = 100
    }
}