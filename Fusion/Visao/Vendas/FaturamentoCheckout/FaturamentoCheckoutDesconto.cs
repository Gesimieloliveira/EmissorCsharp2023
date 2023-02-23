using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Desconto;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickDesconto(object sender, RoutedEventArgs e)
        {
            AcaoAplicarDesconto();
        }

        private void AcaoAplicarDesconto()
        {
            try
            {
                var childModel = ViewModel.CriaContextoDesconto();
                var childView = new AplicarDescontoView(childModel);

                childModel.AplicouDesconto += (sender, args) =>
                {
                    ViewModel.AplicarDescontoPercentual(childModel.Percentual);
                    childView.Close(true);
                    Keyboard.Focus(ControlCheckoutBox);
                };

                AbrirChildWindow(childView);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}