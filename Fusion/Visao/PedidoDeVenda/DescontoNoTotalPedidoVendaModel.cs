using System;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public struct DescontoArgs
    {
        public decimal TotalDesconto { get; set; }
        public decimal Percentual { get; set; }
    }

    public sealed class DescontoNoTotalPedidoVendaModel : ViewModel
    {
        public decimal TotalProdutos
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal Percentual
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 6));

                var novoDesconto = decimal.Round(TotalProdutos * (value / 100), 2);
                var novoTotal = TotalProdutos - novoDesconto;

                SetValue(novoDesconto, nameof(TotalDesconto));
                SetValue(novoTotal, nameof(TotalPedido));
            }
        }

        public decimal TotalDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);

                var novoTotal = decimal.Round(TotalProdutos - value, 2);
                var novoPercentual = decimal.Round(100 - novoTotal * 100 / TotalProdutos, 6);

                SetValue(novoPercentual, nameof(Percentual));
                SetValue(novoTotal, nameof(TotalPedido));
            }
        }

        public decimal TotalPedido
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public event EventHandler<DescontoArgs> DescontoAplicado;

        public void Update(decimal totalProdutos, decimal percentualDesconto)
        {
            TotalProdutos = totalProdutos;
            Percentual = percentualDesconto;
        }

        public void AplicarDesconto()
        {
            if (TotalPedido <= 0)
            {
                throw new InvalidOperationException("Total do documento não pode ficar negativo");
            }

            var args = new DescontoArgs
            {
                Percentual = Percentual,
                TotalDesconto = TotalDesconto
            };

            DescontoAplicado?.Invoke(this, args);
        }
    }
}