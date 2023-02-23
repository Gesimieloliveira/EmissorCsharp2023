using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.Produto;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MovimentacaoEstoque.Flyouts
{
    public partial class MovimentoItemFlyout
    {
        private MovimentoItemFlyoutModel _viewModel;

        public MovimentoItemFlyout()
        {
            InitializeComponent();
        }

        private void IsOpenChangedHandler(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as MovimentoItemFlyoutModel;

            if (_viewModel == null || _viewModel.IsOpen == false)
            {
                _viewModel = null;
                return;
            }

            InputBuscaTextBox.Focus();
        }

        private void FlyoutOnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
                return;

            if (_viewModel != null)
                _viewModel.IsOpen = false;
        }

        private void ClickBotaoSalvarHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.SalvarItem();
        }

        private void ClickBuscaProdutoHandler(object sender, RoutedEventArgs e)
        {
            var model = new ProdutoGridPickerModel();
            model.PickItemEvent += PickerProdutoSelecionadoHandler;

            model.GetPickerView().ShowDialog();
        }

        private void PickerProdutoSelecionadoHandler(object sender, GridPickerEventArgs e)
        {
            var produto = e.GetItem<ProdutoDTO>();
            _viewModel.CarregarDadosComProduto(produto);

            TbQuantidade.Focus();
        }

        private void InputBuscaLostFocusHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                InputBuscaTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                _viewModel.CarregarItemPelaBusca();
            }
            catch (InvalidOperationException ex)
            {
                e.Handled = true;
                InputBuscaTextBox.Focus();

                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}