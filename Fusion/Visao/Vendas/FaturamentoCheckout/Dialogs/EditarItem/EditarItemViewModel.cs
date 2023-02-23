using System;
using System.Data;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.EditarItem
{
    public class EditarItemViewModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly FaturamentoProduto _item;

        public EditarItemViewModel(ISessaoManager sessaoManager, FaturamentoProduto item)
        {
            _sessaoManager = sessaoManager;
            _item = item;
        }

        public short NumeroItem
        {
            get => GetValue<short>();
            private set => SetValue(value);
        }

        public string NomeProduto
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public decimal Quantidade
        {
            get => GetValue<decimal>();
            private set
            {
                SetValue(value);
                CalcularDesconto();
                CalcularTotal();
            }
        }

        public decimal ValorUnitario
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalcularDesconto();
                CalcularTotal();
            }
        }

        public decimal ValorTotal
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                var vUnitario = 0.00M;

                try
                {
                    vUnitario = value / Quantidade;
                }
                catch (DivideByZeroException)
                {
                    // ignore
                }

                SetValue(vUnitario, nameof(ValorUnitario));
                SetValue(0.00M, nameof(PercentualDesconto));
                SetValue(0.00M, nameof(TotalDesconto));
            }
        }

        public decimal PercentualDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(decimal.Round(value, 4));
                PropriedadeAlterada();
                CalcularDesconto();
                CalcularTotal();
            }
        }

        public decimal TotalDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                PropriedadeAlterada();
                CalcularPercentualDesconto();
                CalcularTotal();
            }
        }

        public event EventHandler<FaturamentoProduto> EditadoSucesso;

        public void Inicializar()
        {
            SetValue(_item.Numero, nameof(NumeroItem));
            SetValue(_item.Produto.Nome, nameof(NomeProduto));
            SetValue(_item.Quantidade, nameof(Quantidade));
            SetValue(_item.PrecoUnitario, nameof(ValorUnitario));
            SetValue(_item.PercentualDesconto, nameof(PercentualDesconto));
            SetValue(_item.TotalDesconto, nameof(TotalDesconto));
            SetValue(_item.TotalBruto, nameof(ValorTotal));
        }

        public void AplicarAlteracoes()
        {
            if (Quantidade <= 0)
                throw new InvalidOperationException("Quantidade precisa ser maior que zero");

            if (ValorTotal <= 0)
                throw new InvalidOperationException("Valor Total precisa ser maior que zero");

            if (PercentualDesconto < 0)
                throw new InvalidOperationException("Você não pode usar a % Desconto negativa");

            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioFaturamento(sessao);

                _item.Alterar(ValorUnitario, PercentualDesconto);
                _item.Faturamento.CalcularTotais();

                repositorio.Salvar(_item.Faturamento);
                sessao.Transaction.Commit();
            }

            EditadoSucesso?.Invoke(this, _item);
        }

        private void CalcularDesconto()
        {
            var bruto = ValorUnitario * Quantidade;
            var desconto = bruto * PercentualDesconto / 100;
            SetValue(desconto, nameof(TotalDesconto));
        }

        private void CalcularTotal()
        {
            var bruto = ValorUnitario * Quantidade;
            var total = decimal.Round(bruto - TotalDesconto, 4);

            SetValue(total, nameof(ValorTotal));
        }

        private void CalcularPercentualDesconto()
        {
            try
            {
                var bruto = ValorUnitario * Quantidade;
                var percnetual = (TotalDesconto / bruto) * 100;

                SetValue(decimal.Round(percnetual, 6), nameof(PercentualDesconto));
            }
            catch (DivideByZeroException)
            {
                SetValue(0.00M, nameof(PercentualDesconto));
            }
        }
    }
}