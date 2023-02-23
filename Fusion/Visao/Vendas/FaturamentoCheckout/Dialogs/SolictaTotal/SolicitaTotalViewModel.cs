using System;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.SolictaTotal
{
    public class SolicitaTotalViewModel : ViewModel
    {
        public string NomeItem
        {
            get => GetValue();
            set => SetValue(value);
        }

        public decimal ValorTotal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public event EventHandler<SolicitaTotalViewModel> ConfirmouValor;

        public void AplicarTotal()
        {
            if (ValorTotal <= 0)
            {
                throw new InvalidOperationException("Preciso que informe um total maior que zero");
            }

            ConfirmouValor?.Invoke(this, this);
        }
    }
}