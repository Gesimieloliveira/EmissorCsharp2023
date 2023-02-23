using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model;
using FusionCore.Repositorio.Dtos.Consultas;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba
{
    public partial class AbaPerfilCteOs
    {
        private AbaCteOsPerfilCteOsModel _viewModel;

        public AbaPerfilCteOs()
        {
            InitializeComponent();
        }

        private void SelecionaPerfil_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SelecionarPerfil();
        }

        private void ClickSelecionaPerfil(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                _viewModel.ItemSelecionado = (AbaPerfilCteOsDTO)button.Tag;
                _viewModel.SelecionarPerfil();
            }
        }

        private void AbaPerfilCte_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as AbaCteOsPerfilCteOsModel;
        }
    }
}
