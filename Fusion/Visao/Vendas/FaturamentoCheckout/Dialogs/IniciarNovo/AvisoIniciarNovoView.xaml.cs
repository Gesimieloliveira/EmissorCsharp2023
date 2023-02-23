using System.Windows;
using System.Windows.Input;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.IniciarNovo
{
    public partial class AvisoIniciarNovoView
    {
        public AvisoIniciarNovoView(AvisoIniciarNovoViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            AtalhoBinder.Iniciar(this)
                .BindBotao(Key.Enter, BtnConfirmar)
                .BindBotao(Key.Escape, BtnCancelar);
        }

        public readonly AvisoIniciarNovoViewModel ViewModel;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void OnClickConfirmar(object sender, RoutedEventArgs e)
        {
            ViewModel.ConfirmarNovo();
            Close(true);
        }

        private void OnClickCancelar(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}