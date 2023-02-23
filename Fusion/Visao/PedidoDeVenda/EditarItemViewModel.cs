using System;
using FusionCore.FusionAdm.PedidoVenda;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public sealed class EditarItemViewModel : ViewModel
    {
        private decimal _quantidadeAntiga;

        public bool IsPedidoVenda
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string NomeProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal Quantidade
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 4));
                SetValue(0.00M, nameof(TotalDesconto));
                SetValue(0.00M, nameof(PercentualDesconto));

                CalcularValorTotal();
            }
        }

        public string SiglaUnidade
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal PrecoUnitario
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 4));
                SetValue(0.00M, nameof(TotalDesconto));
                SetValue(0.00M, nameof(PercentualDesconto));

                CalcularValorTotal();
            }
        }

        public decimal PercentualDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 6));

                if (value > 100 || value < 0)
                {
                    SetValue(0M, nameof(TotalDesconto));
                    SetValue(0M, nameof(ValorTotal));

                    return;
                }

                var baseCalculo = decimal.Round(Quantidade * PrecoUnitario, 2);
                var totalDesconto = decimal.Round(baseCalculo * PercentualDesconto / 100, 2);
                var valorTotal = baseCalculo - totalDesconto;

                SetValue(totalDesconto, nameof(TotalDesconto));
                SetValue(valorTotal, nameof(ValorTotal));
            }
        }

        public decimal TotalDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 2));

                try
                {
                    var baseCalculo = decimal.Round(Quantidade * PrecoUnitario, 2);
                    var percentual = decimal.Round(TotalDesconto * 100 / baseCalculo, 6);
                    var valorTotal = decimal.Round(baseCalculo - TotalDesconto, 2);

                    SetValue(percentual, nameof(PercentualDesconto));
                    SetValue(valorTotal, nameof(ValorTotal));
                }
                catch (DivideByZeroException e)
                {
                    SetValue(0M, nameof(PercentualDesconto));
                    SetValue(0M, nameof(ValorTotal));
                }
            }
        }

        public decimal ValorTotal
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 2));
                SetValue(0.00M, nameof(PercentualDesconto));
                SetValue(0.00M, nameof(TotalDesconto));

                try
                {
                    var vUnitario = ValorTotal / Quantidade;

                    SetValue(vUnitario, nameof(PrecoUnitario));
                }
                catch (DivideByZeroException)
                {
                    SetValue(0M, nameof(PrecoUnitario));
                }
            }
        }

        private void CalcularValorTotal()
        {
            var total = decimal.Round(Quantidade * PrecoUnitario - TotalDesconto, 2);

            SetValue(total, nameof(ValorTotal));
        }

        public string Observacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal PrecoVenda
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public event EventHandler<ItemValue> CompletoSucesso;

        public void UpdateModel(PedidoVendaProduto item)
        {
            SetValue(item.Produto.Nome, nameof(NomeProduto));
            SetValue(item.Quantidade, nameof(Quantidade));
            SetValue(item.SiglaUnidade, nameof(SiglaUnidade));
            SetValue(item.PrecoUnitario, nameof(PrecoUnitario));
            SetValue(item.PercentualDesconto, nameof(PercentualDesconto));
            SetValue(item.TotalDesconto, nameof(TotalDesconto));
            SetValue(item.Total, nameof(ValorTotal));
            SetValue(item.Observacao, nameof(Observacao));
            SetValue(item.PrecoVenda, nameof(PrecoVenda));

            _quantidadeAntiga = item.Quantidade;
        }

        public void AplicarAlteracoes()
        {
            ThrowExceptionSeModeloInvalido();

            var itemArgs = new ItemValue
            {
                ValorUnitario = PrecoUnitario,
                PorcentagemDesconto = PercentualDesconto,
                Quantidade = Quantidade,
                TotalDesconto = TotalDesconto,
                TotalLiquido = ValorTotal,
                Observacao = Observacao,
                QuantidadeAntiga = _quantidadeAntiga,
            };

            CompletoSucesso?.Invoke(this, itemArgs);
        }

        private void ThrowExceptionSeModeloInvalido()
        {
            if (Quantidade <= 0)
            {
                throw new InvalidOperationException("Você precisa informar uma quantidade acima de 0");
            }

            if (ValorTotal <= 0)
            {
                throw new InvalidOperationException("Você precisa informar um Valor Total acima de 0");
            }

            if (PercentualDesconto < 0)
            {
                throw new InvalidOperationException("Você não pode usar a % Desconto negativa");
            }
        }
    }
}