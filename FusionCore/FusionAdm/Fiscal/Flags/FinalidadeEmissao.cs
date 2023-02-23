using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum FinalidadeEmissao
    {
        [Description("Normal")] Normal = 1,
        [Description("Complementar")] Complementar = 2,
        [Description("Ajuste")] Ajuste = 3,
        [Description("Devolução")] Devolucao = 4
    }
}