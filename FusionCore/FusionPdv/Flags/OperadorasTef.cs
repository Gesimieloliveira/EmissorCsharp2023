using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionPdv.Flags
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum OperadorasTef
    {
        [Description("Nenhuma")] Nenhuma = 0,
        [Description("Tef Cappta")] Cappta = 1,
        [Description("Tef Ntk")] Ntk = 2
    }
}