using System.Windows;
using System.Windows.Controls;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.NfeInutilizacaoNumeracao
{
    public partial class NfeInutilizacaoNumeracaoForm
    {
        public NfeInutilizacaoNumeracaoForm(NfeInutilizacaoNumeracaoDTO inutilizacao)
        {
            DataContext = new NfeInutilizacaoNumeracaoVm(inutilizacao);
            InitializeComponent();
        }

        private NfeInutilizacaoNumeracaoVm ViewModel => DataContext as NfeInutilizacaoNumeracaoVm;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
        }

        private void ClickInutilizarHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.SolicitaInutilizacao(Dispatcher);
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            ViewModel.Validation_Error(sender, e);
        }
    }
}