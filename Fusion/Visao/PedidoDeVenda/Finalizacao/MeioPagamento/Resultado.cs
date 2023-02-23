using System;
using FusionCore.FusionAdm.PedidoVenda;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento
{
    public class Resultado
    {
        private Resultado(Negociacao negociacao)
        {
            Negociacao = negociacao;
        }

        private Resultado(Exception error)
        {
            Error = error;
        }

        public Negociacao Negociacao { get; }
        public Exception Error { get; }
        public bool HasError => Error != null;

        public static Resultado Sucesso(Negociacao especie)
        {
            return new Resultado(especie);
        }

        public static Resultado Falha(Exception ex)
        {
            return new Resultado(ex);
        }
    }
}