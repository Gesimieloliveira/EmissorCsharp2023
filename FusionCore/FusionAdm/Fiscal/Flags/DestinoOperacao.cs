using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum DestinoOperacao
    {
        [Description("1 - Operação interna")]
        Interna = 1,

        [Description("2 - Operação interestadual")]
        Interestadual = 2,

        [Description("3 - Operação exterior")]
        Exterior = 3
    }
}