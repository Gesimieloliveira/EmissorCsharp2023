using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum IndicadorOperacaoFinal
    {
        [Description("0 - Normal")]
        Normal = 0,

        [Description("1 - Consumidor Final")]
        ConsumidorFinal = 1
    }
}