using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum IndicadorIE
    {
        [Description("Contribuinte ICMS")]
        ContribuinteIcms = 1,

        [Description("Contribuinte Isento")]
        Isento = 2,

        [Description("Não Contribuinte")]
        NaoContribuinte = 9
    }
}