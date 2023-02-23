using System;
using FusionCore.Core.Flags;

namespace FusionCore.ControleCaixa
{
    public class OperacaoCaixa
    {
        public OperacaoCaixa(
            DateTime dataOperacao, 
            ETipoPagamento tipoPagamento, 
            EOrigemFluxoCaixaIndividual origemEvento,
            decimal valor)
        {
            DataOperacao = dataOperacao;
            TipoPagamento = tipoPagamento;
            OrigemEvento = origemEvento;
            Valor = valor;
        }

        public DateTime DataOperacao { get; set; }
        public ETipoPagamento TipoPagamento { get; }
        public EOrigemFluxoCaixaIndividual OrigemEvento { get; }
        public decimal Valor { get; }
    }
}