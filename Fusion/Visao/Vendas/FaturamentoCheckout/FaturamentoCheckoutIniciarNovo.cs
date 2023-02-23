using System;
using System.Windows;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.IniciarNovo;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickIniciarNovo(object sender, RoutedEventArgs e)
        {
            AcaoIniciarNovoFaturamento();
        }

        private void AcaoIniciarNovoFaturamento()
        {
            if (!ViewModel.PossuiFaturamento) return;

            var childModel = new AvisoIniciarNovoViewModel();
            var childView = new AvisoIniciarNovoView(childModel);

            childModel.ConfirmouNovo += ConfirmouIniciarNovoHandle;

            AbrirChildWindow(childView);

            void ConfirmouIniciarNovoHandle(object sender, EventArgs e)
            {
                ViewModel.IniciarNovoFaturamento();
            }
        }
    }
}