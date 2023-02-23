using System.Windows;
using System.Windows.Input;
using Fusion.Helpers;

namespace Fusion.Visao.Pessoa
{
    public partial class PessoaGridControl
    {
        private PessoaGridModel ViewModel => DataContext as PessoaGridModel;

        public PessoaGridControl(PessoaGridModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel?.Inicializar();
        }

        private void ClickNovoHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.NovaPessoa();
        }

        private void DoubleClickRowHandler(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AlterarSelecionado();
        }

        private void AplicarFiltroManipulador(object sender, RoutedEventArgs e)
        {
            ViewModel.AplicarPesquisa();
        }
    }
}
