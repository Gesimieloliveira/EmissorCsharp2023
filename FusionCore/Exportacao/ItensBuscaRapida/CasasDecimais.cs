using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Exportacao.ItensBuscaRapida
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum CasasDecimais
    {
        [Description("Duas R$ 1.12")]
        Duas = 2,

        [Description("Três R$ 1.123")]
        Tres = 3,

        [Description("Quatro R$ 1.1234")]
        Quatro = 4
    }
}