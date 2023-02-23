using System;
using FusionCore.FusionNfce.Produto;
using FusionLibrary.VisaoModel;

namespace FusionNfce.Visao.Principal.SolicitaTotal
{
    public sealed class SolicitaTotalContexto : ViewModel
    {
        private readonly ProdutoNfce _produto;

        public SolicitaTotalContexto(ProdutoNfce produto, decimal quantidaeInicial)
        {
            QuantidadeCalculada = quantidaeInicial;
            ValorTotal = decimal.Round(produto.PrecoVenda * quantidaeInicial, 2);

            _produto = produto;
        }

        public event EventHandler<SolicitaTotalContexto> FinalizadoSucesso;

        public decimal? ValorTotal
        {
            get => GetValue<decimal?>();
            set => SetValue(value);
        }

        public decimal QuantidadeCalculada
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public void CalcularQuantidade(decimal? valorTotal)
        {
            if (valorTotal.HasValue)
            {
                const int KG = 1000;

                var kgValorVenda = KG / _produto.PrecoVenda;
                var qtdeKg = (kgValorVenda * (decimal)valorTotal) / KG;

                QuantidadeCalculada = decimal.Round(qtdeKg, 4);
                return;
            }

            QuantidadeCalculada = 0.00M;
        }

        public void FianlizarSolicitacao()
        {
            if (ValorTotal == null || ValorTotal <= 0)
            {
                throw new InvalidOperationException("Preciso de um Valor Total válido");
            }

            if (QuantidadeCalculada <= 0)
            {
                throw new InvalidOperationException("Preciso de uma Quantidade válida");
            }

            FinalizadoSucesso?.Invoke(this, this);
        }
    }
}