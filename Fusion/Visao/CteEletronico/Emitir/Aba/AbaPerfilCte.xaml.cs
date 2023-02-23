using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using Fusion.Visao.CteEletronico.Perfil;
using FusionCore.FusionAdm.CteEletronico;
using FusionCore.Repositorio.Dtos.Consultas;

namespace Fusion.Visao.CteEletronico.Emitir.Aba
{
    public partial class AbaPerfilCte
    {
        private AbaPerfilCteModel _viewModel;

        public AbaPerfilCte()
        {
            InitializeComponent();
        }

        private void NovoPerfil_OnMouseUp(object sender, RoutedEventArgs e)
        {
            new CtePerfilForm(new CtePerfilFormModel(new PerfilCte())).ShowDialog();
            _viewModel.Inicializar();
        }

        private void DoubleClickListItem(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SelecionaPerfil();
        }

        private void Pesquisa_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            _viewModel.EfetuaPesquisaRapida();
        }

        private void AbaPerfilCte_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as AbaPerfilCteModel;
            _viewModel?.Inicializar();
        }

        private void ClickSelecionaPerfil(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                _viewModel.ItemSelecionado = (PerfilCteListBoxDTO) button.Tag;
                _viewModel.SelecionaPerfil();
            }
        }
    }
}