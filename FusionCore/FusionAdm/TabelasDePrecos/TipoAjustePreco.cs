using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoAjustePreco
    {
        [Description("Acréscimo no preço de venda")]
        AcrecimoPrecoVenda = 0,

        [Description("Desconto no preço de venda")]
        DescontoPrecoVenda = 1,
    }
}