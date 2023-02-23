using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum StatusNfe
    {
        [Description("Pendente")]
        Pendente = 1,

        [Description("Autorizada")]
        Autorizada = 2,

        [Description("Cancelada")]
        Cancelada = 3,

        [Description("Denegada")]
        Denegada = 4
    }
}