using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoPeriodoData
    {
        [Description("Sem data prevista")]
        SemDataDefinida,
        [Description("Na data")]
        NaData,
        [Description("Até a data")]
        AteAData,
        [Description("A partir da data")]
        APartirDaData,
        [Description("No período")]
        NoPeriodo
    }
}