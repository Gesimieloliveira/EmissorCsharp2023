using System;
using System.Windows;
using FusionCore.FusionAdm.Compras;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public class TotaisCompraChildViewModel : ViewModel
    {
        private readonly NotaFiscalCompra _nota;

        public decimal ValorFrete
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorSeguro
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDespesas
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool IsOpen
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public TotaisCompraChildViewModel(NotaFiscalCompra nota = null)
        {
            _nota = nota;
        }

        public event EventHandler<TotaisCompraChildViewModel> ConfirmouValores;

        public void Inicializar()
        {
            ValorFrete = _nota?.ValorTotalFrete ?? 0.00M;
            ValorSeguro = _nota?.ValorTotalSeguro ?? 0.00M;
            ValorDespesas = _nota?.ValorTotalOutros ?? 0.00M;
        }

        private void OnConfirmouValores(TotaisCompraChildViewModel e)
        {
            ConfirmouValores?.Invoke(this, e);
        }

        public void ConfirmaAlteracoes()
        {
            if (ValorSeguro < 0 || ValorDespesas < 0 || ValorFrete < 0)
            {
                DialogBox.MostraAviso("Valores devem ser positivos");
            }

            if (!DialogBox.MostraConfirmacao(
                "Valor Seguro, Valor Despesas e Valor Frete vão substituir os valores atuais dos itens\n" +
                "Deseja realmente realizar a operação?",
                MessageBoxImage.Warning)) return;

            OnConfirmouValores(this);
        }
    }
}