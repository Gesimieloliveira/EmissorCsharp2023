using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Vendas.Faturamentos
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum LayoutImpressao
    {
        [Description("Impressão 80 mm")]
        Impressao80M = 0,

        [Description("Impressão A4")]
        ImpressaoA4 = 1
    }
}