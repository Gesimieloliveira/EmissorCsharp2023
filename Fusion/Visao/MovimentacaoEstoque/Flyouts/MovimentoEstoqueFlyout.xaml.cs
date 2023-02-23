using System;
using System.Windows;

namespace Fusion.Visao.MovimentacaoEstoque.Flyouts
{
    public partial class MovimentoEstoqueFlyout
    {
        private MovimentoEstoqueFlyoutModel _viewmModel;

        public MovimentoEstoqueFlyout()
        {
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _viewmModel = DataContext as MovimentoEstoqueFlyoutModel;
            _viewmModel?.CarregaDados();

            if (_viewmModel != null)
                _viewmModel.OperacaoFinalizada += OperacaoFinalizadaHandler;
        }

        private void OperacaoFinalizadaHandler(object sender, EventArgs e)
        {
            _viewmModel.IsOpen = false;
        }

        private void OnClickSalvarMovimentacao(object sender, RoutedEventArgs e)
        {
            _viewmModel?.SavarAlteracoes();
        }
    }
}