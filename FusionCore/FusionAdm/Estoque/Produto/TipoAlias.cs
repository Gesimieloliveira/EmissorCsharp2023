using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Estoque.Produto
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoAlias
    {
        [Description("Código de Barras")]
        CodigoBarra,

        [Description("Código")]
        Codigo,

        [Description("GTIN")]
        Gtin
    }
}