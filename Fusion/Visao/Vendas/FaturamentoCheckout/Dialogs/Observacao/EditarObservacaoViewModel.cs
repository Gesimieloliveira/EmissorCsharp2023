using System;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Observacao
{
    public class EditarObservacaoViewModel : ViewModel
    {
        public event EventHandler<string> ConfirmouObservacao;

        public string Observacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public void ConfirmarObservacao()
        {
            ConfirmouObservacao?.Invoke(this, Observacao);
        }
    }
}