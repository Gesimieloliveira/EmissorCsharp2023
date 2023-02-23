using System.Windows;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Preferencias;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickPreferencias(object sender, RoutedEventArgs e)
        {
            AbrirPreferencias();
        }

        private void AbrirPreferenciasSeNaoConfigurado()
        {
            if (ViewModel.Preferencias.PossuiPreferenciaParaMaquina())
                return;

            if (!ViewModel.TemPermissaoPreferencias)
            {
                DialogBox.MostraAviso("Preferencias não configurada e usuário não possui permissão");
                return;
            }

            AbrirPreferencias();
        }

        private void AbrirPreferencias()
        {
            var childModel = new PreferenciasViewModel(ViewModel.Preferencias);
            var childView = new PreferenciasView(childModel);

            AbrirChildWindow(childView);
        }
    }
}