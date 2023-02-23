using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MovimentacaoEstoque
{
    public partial class MovimentoEstoqueForm
    {
        private readonly MovimentoEstoqueFormModel _viewModel;

        public MovimentoEstoqueForm(MovimentoEstoqueFormModel model)
        {
            InitializeComponent();
            DataContext = model;
            _viewModel = model;
            _viewModel.OperacaoCancelada += OperacaoCanceladaHandler;
            _viewModel.MovimentoExcluido += MovimentoExcluidoHandler;

        }

        private void MovimentoExcluidoHandler(object sender, EventArgs e)
        {
            DialogBox.MostraInformacao("Movimento foi excluido com sucesso");
            Close();
        }

        private void OperacaoCanceladaHandler(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.CarregarDados();
        }

        private void ClickAdicionarItemHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.AbrirFlyoutMovimentoItem();
        }

        private void ClickExcluirMovimentoHandler(object sender, RoutedEventArgs e)
        {
            var confirmacao = DialogBox.MostraConfirmacao("Deseja excluir a movimentação e seus itens ?");
            if (confirmacao != MessageBoxResult.Yes)
                return;

            _viewModel.DeletarMovimentacao();
        }

        private void ClickExcluirItemHandler(object sender, MouseButtonEventArgs e)
        {
            var confirmacao = DialogBox.MostraConfirmacao("Deseja excluir o item selecionado ?");
            if (confirmacao != MessageBoxResult.Yes)
                return;

            _viewModel.DeletarItemSelecionado();
        }
    }
}