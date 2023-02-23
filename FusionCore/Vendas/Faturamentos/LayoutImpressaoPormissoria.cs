using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Vendas.Faturamentos
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum LayoutImpressaoPormissoria
    {
        [Description("Impressão em formato promissória")]
        ImpressaoPromissoria = 0,

        [Description("Impressão em formato carnê com promissória")]
        ImpressaoPromissoriaCarne = 1,

        [Description("Não Imprimir")]
        NaoImprimir = 2
    }
}