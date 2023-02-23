using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Financeiro.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoParcela
    {
        [Description("Intervalo")]
        Intervalo,

        [Description("Dia Fixo")]
        DiaFixo
    }
}