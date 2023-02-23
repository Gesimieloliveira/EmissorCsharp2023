using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Observacao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickObservacao(object sender, RoutedEventArgs e)
        {
            AcaoObservacao();
        }

        public void AcaoObservacao()
        {
            try
            {
                var childModel = ViewModel.CriarContextoObservacao();
                var childView = new EditarObservacaoView(childModel);

                childView.ViewModel.ConfirmouObservacao += ConfirmouObservacaoHandler;

                AbrirChildWindow(childView);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ConfirmouObservacaoHandler(object sender, string novaObservacao)
        {
            try
            {
                ViewModel.AlterarObservacao(novaObservacao);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                Keyboard.Focus(ControlCheckoutBox);
            }
        }
    }
}