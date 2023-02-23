using System;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.IniciarNovo
{
    public class AvisoIniciarNovoViewModel : ViewModel
    {
        public event EventHandler ConfirmouNovo;

        public void ConfirmarNovo()
        {
            ConfirmouNovo?.Invoke(this, EventArgs.Empty);
        }
    }
}