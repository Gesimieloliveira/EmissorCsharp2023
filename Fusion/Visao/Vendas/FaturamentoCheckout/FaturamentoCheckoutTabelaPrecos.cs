using System.Windows.Input;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TabelaPrecos;
using FusionCore.FusionAdm.TabelasDePrecos;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void AcaoEscolherTabelaPrecos()
        {
            var childView = new TabelaPrecosView(
                ViewModel.CriarViewModelTabelaPrecos()
            );

            childView.ViewModel.Confirmou += OnConfirmouTabela;

            AbrirChildWindow(childView);

            void OnConfirmouTabela(object sender, ITabelaPreco tabelaDto)
            {
                ViewModel.AplicarTabelaPrecos(tabelaDto);
                ControlCheckoutBox.ComTabelaPrecos(tabelaDto);
            }
        }

        private void TextBoxTabelaPrecos_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            AcaoEscolherTabelaPrecos();
        }
    }
}