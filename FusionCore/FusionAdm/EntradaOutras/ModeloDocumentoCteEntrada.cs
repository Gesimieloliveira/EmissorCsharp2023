using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.EntradaOutras
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ModeloDocumentoCteEntrada
    {
        [Description("CT-e")]
        Cte = 57,

        [Description("CTe OS")]
        CteOs = 67
    }
}