using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models;
using Fusion.Visao.PerfilNfe;
using FusionCore.Repositorio.Dtos.Consultas;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba
{
    public partial class AbaPerfilPicker
    {
        private AbaPerfilPickerModel _viewModel;

        public AbaPerfilPicker()
        {
            InitializeComponent();
        }

        private void SelecionarPerfilNfe_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as AbaPerfilPickerModel;
            _viewModel?.OnLoadedView();
        }

        private void DoubleClickListItem(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SelecionaPerfil();
        }

        private void NovoPerfil_OnMouseUp(object sender, RoutedEventArgs routedEventArgs)
        {
            new PerfilNfeForm(new PerfilNfeFormModel()).ShowDialog();
            _viewModel?.OnLoadedView();
        }

        private void Pesquisa_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            _viewModel.FazerPesquisa();
        }

        private void ClickItemPerfil(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                _viewModel.ItemSelecionado = (AbaPerfilNfeDTO) button.Tag;
                _viewModel.SelecionaPerfil();
            }
        }
    }
}