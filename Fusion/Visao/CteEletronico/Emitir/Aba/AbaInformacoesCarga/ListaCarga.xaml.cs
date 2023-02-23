using System.Windows;
using System.Windows.Input;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.AbaInformacoesCarga
{
    public partial class ListaCarga
    {
        private AbaInformacoesCargaCteModel _viewModel;

        public ListaCarga()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            _viewModel.DeletaCargaSelecionada();
        }

        private void OnDoubleClickItem(object sender, MouseButtonEventArgs e)
        {
        }

        private void ListaCarga_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaInformacoesCargaCteModel;
            if (model == null) return;
            _viewModel = (AbaInformacoesCargaCteModel) DataContext;
        }
    }
}