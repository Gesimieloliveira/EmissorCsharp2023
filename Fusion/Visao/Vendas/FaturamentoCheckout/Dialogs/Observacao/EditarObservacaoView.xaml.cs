using System.Windows;
using System.Windows.Input;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Observacao
{
    public partial class EditarObservacaoView
    {
        public EditarObservacaoView(EditarObservacaoViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public readonly EditarObservacaoViewModel ViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            Keyboard.Focus(TbOservacao);
            TbOservacao.MoveCaretToEnd();
        }

        private void ConfirmarClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoConfirmar();
        }

        private void AcaoConfirmar()
        {
            BotaoConfirmar.Focus();
            ViewModel.ConfirmarObservacao();
            Close(true);
        }

        private void EditarObservacaoView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                e.Handled = true;
                AcaoConfirmar();
            }
        }
    }
}