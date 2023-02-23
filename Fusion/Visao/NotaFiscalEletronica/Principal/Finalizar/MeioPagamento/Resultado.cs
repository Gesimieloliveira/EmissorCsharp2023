using System;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar.MeioPagamento
{
    public class Resultado
    {
        private Resultado(FormaPagamentoNfe pagamento)
        {
            Pagamento = pagamento;
        }

        private Resultado(Exception error)
        {
            Error = error;
        }

        public FormaPagamentoNfe Pagamento { get; }
        public Exception Error { get; }
        public bool HasError => Error != null;

        public static Resultado Sucesso(FormaPagamentoNfe especie)
        {
            return new Resultado(especie);
        }

        public static Resultado Falha(Exception ex)
        {
            return new Resultado(ex);
        }
    }
}