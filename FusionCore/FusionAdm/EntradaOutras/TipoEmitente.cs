using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.EntradaOutras
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoEmitente
    {
        [Description("Próprio")]
        Proprio = 0,

        [Description("Terceiro")]
        Terceiro = 1
    }
}