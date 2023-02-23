using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.AbaInformacoesCarga
{
    public partial class ListaVeiculos
    {
        private AbaInformacoesCargaCteModel _viewModel;

        public ListaVeiculos()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            _viewModel.DeletarVeiculoParaTransporteSelecionado();
        }

        private void ListaVeiculos_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is AbaInformacoesCargaCteModel))
            {
                return;
            }

            _viewModel = (AbaInformacoesCargaCteModel) DataContext;
        }
    }
}