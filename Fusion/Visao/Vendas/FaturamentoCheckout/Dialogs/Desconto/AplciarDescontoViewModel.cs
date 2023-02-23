using System;
using FusionCore.Vendas.Faturamentos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Desconto
{
    public class AplciarDescontoViewModel : ViewModel
    {
        public AplciarDescontoViewModel(FaturamentoVenda faturamento)
        {
            SetValue(faturamento.TotalProdutos, nameof(TotalProdutos));
            SetValue(faturamento.PercentualDesconto, nameof(Percentual));
            SetValue(faturamento.TotalDesconto, nameof(TotalDesconto));
            SetValue(faturamento.Total, nameof(TotalFaturamento));
        }

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
                SetValue(value);

                var novoDesconto = decimal.Round(TotalProdutos * (value / 100), 2);
                var notoTotal = TotalProdutos - novoDesconto;

                SetValue(novoDesconto, nameof(TotalDesconto));
                SetValue(notoTotal, nameof(TotalFaturamento));
            }
        }

        public decimal TotalDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);

                var novoTotal = decimal.Round(TotalProdutos - value, 4);
                var novoPercentual = decimal.Round(100 - novoTotal * 100 / TotalProdutos, 4);

                SetValue(novoPercentual, nameof(Percentual));
                SetValue(novoTotal, nameof(TotalFaturamento));
            }
        }

        public decimal TotalFaturamento
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public event EventHandler AplicouDesconto;

        public void AplicarDesconto()
        {
            if (TotalFaturamento <= 0)
            {
                throw new InvalidOperationException("Total do faturamento não pode ficar negativo");
            }

            AplicouDesconto?.Invoke(this, EventArgs.Empty);
        }
    }
}