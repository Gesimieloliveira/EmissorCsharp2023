using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Financeiro.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum Situacao
    {
        [Description("Aberto")]
        Aberto = 0,

        [Description("Quitado")]
        Quitado = 1,

        [Description("Cancelado")]
        Cancelado = 2
    }
}