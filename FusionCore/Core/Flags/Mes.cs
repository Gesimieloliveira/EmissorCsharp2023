using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Core.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum Mes
    {
        [Description("Janeiro")]
        Janeiro = 1,

        [Description("Fevereiro")]
        Fevereiro = 2,

        [Description("Março")]
        Marco = 3,

        [Description("Abril")]
        Abril = 4,

        [Description("Maio")]
        Maio = 5,

        [Description("Junho")]
        Junho = 6,

        [Description("Julho")]
        Julho = 7,

        [Description("Agosto")]
        Agosto = 8,

        [Description("Setembro")]
        Setembro = 9,

        [Description("Outubro")]
        Outubro = 10,

        [Description("Novembro")]
        Novembro = 11,

        [Description("Dezembro")]
        Dezembro = 12
    }
}