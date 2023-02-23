using System;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento
{
    public class Resultado
    {
        private Resultado(FPagamento especie)
        {
            Pagamento = especie;
        }

        private Resultado(Exception error)
        {
            Error = error;
        }

        public FPagamento Pagamento { get; }
        public Exception Error { get; }
        public bool HasError => Error != null;

        public static Resultado Sucesso(FPagamento pagamento)
        {
            return new Resultado(pagamento);
        }

        public static Resultado Falha(Exception ex)
        {
            return new Resultado(ex);
        }
    }
}