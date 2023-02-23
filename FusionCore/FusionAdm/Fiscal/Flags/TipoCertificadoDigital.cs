using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoCertificadoDigital
    {
        [Description("A1 Arquivo")]
        A1Arquivo,
        [Description("A1 Repositório")]
        A1Repositorio,
        [Description("A3")]
        A3
    }
}