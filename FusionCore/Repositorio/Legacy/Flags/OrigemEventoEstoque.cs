using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Repositorio.Legacy.Flags
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum OrigemEventoEstoque
    {
        [Description("Saldo inicial, adicinado no momento do cadastro do produto")]
        SaldoInicial = 1,
        [Description("Movimentação avulsa nas opções dos produtos")]
        AjusteAvulso = 2,
        [Description("Movimentado apartir de venda item do cupom fiscal")]
        ItemPdv = 3,
        [Description("Movimentado apartir de cancelado de item do cupom fiscal")]
        CancelamentoItemPdv = 4,
        [Description("Movimentado apartir de inserção de item na movimentação de estoque")]
        InsertMovimentoEstoqueItem = 5,
        [Description("Movimentado apartir de remoção de item na movimentação de estoque")]
        DeleteMovimentoEstoqueItem = 6,
        [Description("Movimentado apartir de inserção de item na Nota Fiscal Eletrônica")]
        ItemAdicionadoNfe = 7,
        [Description("Movimentado apartir de remoção de item na Nota Fiscal Eletrônica")]
        ItemRemovidoNfe = 8,
        [Description("Movimento gerado após NF-E ser cancelada pelo usuário")]
        CancelamentoNfe = 9,
        [Description("Movimentado apartir de inserção de item na Nota Fiscal do Consumidor Eletrônica")]
        ItemAdicionadoNfce = 10,
        [Description("Movimentado apartir de remoção de item na Nota Fiscal do Consumidor Eletrônica")]
        ItemRemovidoNfce = 11,
        [Description("Movimentado apartir de cancelamento de Nota Fiscal do Consumidor Eletrônica")]
        CancelamentoNfce = 12,
        [Description("Movimentado aparitr de alteração de CFOP da NF-e")]
        CfopItemAlteradoNfe = 13,
        [Description("Movimentado a partir de alteração de item na Nota Fiscal do Consumidor Eletrônica")]
        MovimentacaoNfceApartirDeAlteracao = 14,
        [Description("Item adicionado na nota fiscal de compra")]
        ItemAdicionadoCompra = 15,
        [Description("Item removido na nota fiscal de compra")]
        ItemRemovidoCompra = 16,
        [Description("Movimento gerado após NF-E ser denegada")]
        DenegacaoNfe = 17,
        [Description("Item adicionado no faturamento")]
        ItemAdicionadoFaturamento = 18,
        [Description("Item removido no faturamento")]
        ItemRemovidoFaturamento = 19,
        [Description("Item cancelado devido a cancelamento do faturamento")]
        FaturamentoCancelado = 20,

        [Description("Item reservado no pedido de venda")]
        ItemAdicionadoPedidoVenda = 21,

        [Description("Item removido da reserva no pedido de venda")]
        ItemRemovidoPedidoVenda = 22,

        [Description("Item removido da reserva devido a cancelamento do pedido de venda")]
        PedidoVendaCancelado = 23,

        [Description("Baixa de item reservado após faturamento do pedido de venda em NF-e")]
        PedidoVendaReservaEfetuadaNfe = 24,

        [Description("Baixa de item por orçamento após faturamento do pedido de venda em NF-e")]
        PedidoVendaOrcamentoEfetuadaNfe = 25,

        [Description("Baixa de item reservado após faturamento do pedido de venda em NFC-e")]
        PedidoVendaReservaEfetuadaNfce = 26,

        [Description("Baixa de item por orçamento após faturamento do pedido de venda em NFC-e")]
        PedidoVendaOrcamentoEfetuadaNfce = 27,

        [Description("Baixa de item reservado após faturamento do pedido de venda em Faturamento")]
        PedidoVendaReservaEfetuadaFaturamento = 28,

        [Description("Baixa de item por orçamento após faturamento do pedido de venda em Faturmento")]
        PedidoVendaOrcamentoEfetuadaFaturamento = 29,

        [Description("Movimento gerado após NFC-e ser denegada")]
        DenegacaoNfce = 30,

        [Description("Movimentado a partir de alteração de item no Pedido/Orçamento")]
        MovimentacaoPedidoVendaApartirDeAlteracao = 31,

        [Description("Movimentado a partir de alteração no item da Nota Fiscal Eletrônica")]
        ItemAlteradoNfe = 32,
    }
}
