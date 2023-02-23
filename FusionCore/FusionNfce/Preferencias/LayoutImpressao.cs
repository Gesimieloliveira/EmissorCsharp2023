using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionNfce.Preferencias
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum LayoutImpressao
    {
        [Description("Impressão 80 mm")]
        Impressao80M = 0,

        [Description("Impressão 58 mm")]
        Impressao58M = 1,

        [Description("Impressão A4")]
        ImpressaoA4 = 2,
    }
}