using System.Windows;
using FusionWPF.CapturarPeso;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickConfigurarBalanca(object sender, RoutedEventArgs e)
        {
            AbrirJanelaConfiguracaoBalanca();
        }

        private void AbrirJanelaConfiguracaoBalanca()
        {
            var viewModel = new ConfiguracaoBalancaContexto(_sessaoSistema.Preferencias);
            var view = new ConfiguracaoBalanca(viewModel);

            AbrirChildWindow(view);
        }
    }
}