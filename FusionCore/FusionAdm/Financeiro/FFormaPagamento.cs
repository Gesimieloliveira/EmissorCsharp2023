using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Financeiro
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum FFormaPagamento
    {
        [Description("Dinheiro")]
        Dinheiro,

        [Description("Cheque")]
        Cheque,

        [Description("Cartão Crédito")]
        CartaoCredito,

        [Description("Cartão Debito")]
        CartaoDebito,

        [Description("Credito Loja")]
        CreditoLoja,

        [Description("Vale Alimentação")]
        ValeAlimentacao,

        [Description("Vale Refeição")]
        ValeRefeicao,

        [Description("Vale Presente")]
        ValePresente,

        [Description("Vale Combustível")]
        ValeCombustivel,

        [Description("Duplicata Mercantil")]
        DuplicataMercantil,

        [Description("Boleto Bancário")]
        BoletoBancario,

        [Description("Sem Pagamento")]
        SemPagamento,

        [Description("Outra")]
        Outro
    }
}