using System;
using FusionCore.Core;
using FusionCore.FusionAdm.TabelasDePrecos;
using static System.Decimal;

namespace FusionCore.FusionNfce.Fiscal
{
    public class PrecoItem : NotifyPropertyChanged, IAtualizaValorUnitario
    {
        private decimal _quantidade;
        private decimal _totalBruto;
        private decimal _totalLiquido;
        private decimal _valorUnitario;
        private decimal _desconto;
        private decimal _acrescimo;
        private decimal _descontoAlteraItem;
        private decimal _totalUnitarioSemDescontoGeral;

        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                if (_quantidade == value)
                {
                    return;
                }

                _quantidade = Round(value, 4);
                CalcularTotal();
                OnPropertyChanged();
            }
        }

        public decimal Desconto
        {
            get => _desconto;
            set
            {
                if (_desconto == value)
                {
                    return;
                }

                _desconto = Round(value, 2);
                _acrescimo = 0;

                CalcularTotal();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Acrescimo));
            }
        }

        public decimal DescontoAlteraItem
        {
            get => _descontoAlteraItem;
            set
            {
                if (_descontoAlteraItem == value)
                {
                    return;
                }

                _descontoAlteraItem = Round(value, 2);
                _acrescimo = 0;

                CalcularTotal();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Acrescimo));
                OnPropertyChanged(nameof(TotalUnitarioSemDescontoGeral));
            }
        }

        public decimal Acrescimo
        {
            get => _acrescimo;
            set
            {
                if (_acrescimo == value)
                {
                    return;
                }

                _acrescimo = value;
                _desconto = 0;

                CalcularTotal();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Desconto));
            }
        }

        public decimal ValorUnitario
        {
            get => _valorUnitario;
            set
            {
                if (_valorUnitario == value)
                {
                    return;
                }

                _valorUnitario = decimal.Round(value, 6);
                CalcularTotal();
                OnPropertyChanged();
            }
        }

        public decimal TotalLiquido
        {
            get => _totalLiquido;
            set
            {
                if (_totalLiquido == value)
                {
                    return;
                }

                _totalLiquido = decimal.Round(value, 2);

                CalcularDiferenca();
                CalcularTotalBruto();
                OnPropertyChanged();
            }
        }

        public decimal TotalUnitarioSemDescontoGeral
        {
            get => _totalUnitarioSemDescontoGeral;
            set
            {
                _totalUnitarioSemDescontoGeral = value;

                _descontoAlteraItem = 0.0m;
                _valorUnitario = value / _quantidade;

                OnPropertyChanged(nameof(TotalUnitarioSemDescontoGeral));
                OnPropertyChanged(nameof(DescontoAlteraItem));
                OnPropertyChanged(nameof(ValorUnitario));
                CalcularTotal();
            }
        }

        public decimal TotalBruto => _totalBruto;

        private void CalcularDiferenca()
        {
            _acrescimo = 0.00M;
            _desconto = 0.00M;

            var totalBrutoPrevisto = Round(_quantidade * _valorUnitario, 2);

            if (_totalLiquido > totalBrutoPrevisto)
            {
                _acrescimo = _totalLiquido - totalBrutoPrevisto;
            }

            if (_totalLiquido < totalBrutoPrevisto)
            {
                _desconto = totalBrutoPrevisto - _totalLiquido;
            }

            OnPropertyChanged(nameof(Desconto));
            OnPropertyChanged(nameof(Acrescimo));
        }

        private void CalcularTotal()
        {
            CalcularTotalBruto();
            CalcularTotalLiquido();
            CalcularTotalUnitario();
        }

        private void CalcularTotalUnitario()
        {
            _totalUnitarioSemDescontoGeral = Round((ValorUnitario * Quantidade) - DescontoAlteraItem, 2);
            OnPropertyChanged(nameof(TotalUnitarioSemDescontoGeral));
        }

        private void CalcularTotalBruto()
        {
            _totalBruto = Round(_quantidade * _valorUnitario, 2);
            OnPropertyChanged(nameof(TotalBruto));
        }

        private void CalcularTotalLiquido()
        {
            _totalLiquido = Round(_totalBruto + _acrescimo - (_desconto + _descontoAlteraItem), 2);
            OnPropertyChanged(nameof(TotalLiquido));
        }

        public static PrecoItem Factory(decimal quantidade,
            decimal valorUnitario,
            decimal desconto,
            decimal acrescimo,
            decimal totalBruto,
            decimal totalLiquido,
            decimal descontoAlteraItem,
            decimal totalUnitarioSemDescontoGeral)
        {
            return new PrecoItem
            {
                _quantidade = Round(quantidade, 4),
                _valorUnitario = Round(valorUnitario, 6),
                _desconto = Round(desconto, 2),
                _acrescimo = Round(acrescimo, 2),
                _totalLiquido = Round(totalLiquido, 2),
                _totalBruto = Round(totalBruto, 2),
                _descontoAlteraItem =  Round(descontoAlteraItem, 2),
                _totalUnitarioSemDescontoGeral = Round(totalUnitarioSemDescontoGeral, 4)
            };
        }

        public static PrecoItem Factory(decimal quantidade, decimal valorUnitario)
        {
            return new PrecoItem()
            {
                Quantidade = Round(quantidade, 4),
                ValorUnitario = Round(valorUnitario, 4)
            };
        }

        public void ThrowExceptionSeInvalido()
        {
            if (TotalLiquido <= 0)
            {
                throw new InvalidOperationException("Total precisa ser maior que zero");
            }
        }

        public void AtualizarValorUnitario(decimal novoValorUnitario)
        {
            ValorUnitario = novoValorUnitario;
        }
    }
}