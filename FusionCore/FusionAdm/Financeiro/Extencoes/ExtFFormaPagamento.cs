using NFe.Classes.Informacoes.Pagamento;

namespace FusionCore.FusionAdm.Financeiro.Extencoes
{
    public static class ExtFFormaPagamento
    {
        public static FormaPagamento ToZeusNfe(this FFormaPagamento pag)
        {
            switch (pag)
            {
                case FFormaPagamento.Dinheiro:
                    return FormaPagamento.fpDinheiro;
                case FFormaPagamento.Cheque:
                    return FormaPagamento.fpCheque;
                case FFormaPagamento.CartaoCredito:
                    return FormaPagamento.fpCartaoCredito;
                case FFormaPagamento.CartaoDebito:
                    return FormaPagamento.fpCartaoDebito;
                case FFormaPagamento.CreditoLoja:
                    return FormaPagamento.fpCreditoLoja;
                case FFormaPagamento.ValeAlimentacao:
                    return FormaPagamento.fpValeAlimentacao;
                case FFormaPagamento.ValeRefeicao:
                    return FormaPagamento.fpValeRefeicao;
                case FFormaPagamento.ValePresente:
                    return FormaPagamento.fpValePresente;
                case FFormaPagamento.ValeCombustivel:
                    return FormaPagamento.fpValeCombustivel;
                case FFormaPagamento.DuplicataMercantil:
                    return FormaPagamento.fpDuplicataMercantil;
                case FFormaPagamento.BoletoBancario:
                    return FormaPagamento.fpBoletoBancario;
                case FFormaPagamento.SemPagamento:
                    return FormaPagamento.fpSemPagamento;
                case FFormaPagamento.Outro:
                    return FormaPagamento.fpOutro;
            }

            return FormaPagamento.fpOutro;
        }
    }
}