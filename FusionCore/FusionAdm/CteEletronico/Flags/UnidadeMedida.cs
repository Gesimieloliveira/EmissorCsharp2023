using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum UnidadeMedida
    {
        [Description("M3")]
        M3 = 0,
        [Description("Kg")]
        Kg = 1,
        [Description("Ton")]
        Ton = 2,
        [Description("Unidade")]
        Unidade = 3,
        [Description("Litros")]
        Litros = 4,
        [Description("Mmbtu")]
        Mmbtu = 5
    }
}