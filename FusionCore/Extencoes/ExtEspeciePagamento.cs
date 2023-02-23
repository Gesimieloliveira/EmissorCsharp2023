using FusionCore.Core.Flags;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Pagamento;

// ReSharper disable SwitchStatementMissingSomeCases

namespace FusionCore.Extencoes
{
    public static class ExtEspeciePagamento
    {
        public static bool EhCartao(this ETipoPagamento especie)
        {
            switch (especie)
            {
                case ETipoPagamento.CartaoDebito:
                case ETipoPagamento.CartaoCredito:
                    return true;
                default:
                    return false;
            }
        }

        public static FormaPagamento ToZeusPagamento(this ETipoPagamento especie)
        {
            switch (especie)
            {
                case ETipoPagamento.Dinheiro: return FormaPagamento.fpDinheiro;
                case ETipoPagamento.CreditoLoja: return FormaPagamento.fpCreditoLoja;
                case ETipoPagamento.CartaoCredito: return FormaPagamento.fpCartaoCredito;
                case ETipoPagamento.CartaoDebito: return FormaPagamento.fpCartaoDebito;
                case ETipoPagamento.Pix: return FormaPagamento.fpPagamentoInstantaneoPIX;
                default: return FormaPagamento.fpSemPagamento;
            }
        }

        public static IndicadorPagamentoDetalhePagamento ToZeusIndicadorPg(this ETipoPagamento especie)
        {
            switch (especie)
            {
                case ETipoPagamento.CreditoLoja:
                    return IndicadorPagamentoDetalhePagamento.ipDetPgPrazo;
                default:
                    return IndicadorPagamentoDetalhePagamento.ipDetPgVista;
            }
        }
    }
}