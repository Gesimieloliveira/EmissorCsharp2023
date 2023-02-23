using System.ComponentModel;

namespace FusionCore.FusionAdm.PedidoVenda
{
    public enum TipoPedido
    {
        [Description("Pedido de Venda")]
        PedidoVenda = 0,

        [Description("Orçamento")]
        Orcamento = 1
    }
}