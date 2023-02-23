using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Facades;
using Fusion.Sessao;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public interface IFaturamentoView
    {
        FaturamentoCheckoutViewModel ViewModel { get; }
        void ShowView();
    }

    public partial class FaturamentoCheckout : IFaturamentoView
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly ImpressorFaturamento _impressor;

        private FaturamentoCheckout(FaturamentoCheckoutViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            _impressor = new ImpressorFaturamento(_sessaoSistema.SessaoManager);

            AtalhoBinder.Iniciar(this)
                .BindAction(Key.F2, AcaoCliente, NaoPossuiChildAberta)
                .BindAction(Key.F3, AcaoCancelarFaturamento, NaoPossuiChildAberta)
                .BindAction(Key.F4, AcaoFinalizar, NaoPossuiChildAberta)
                .BindAction(Key.F5, AcaoObservacao, NaoPossuiChildAberta)
                .BindAction(Key.F6, AcaoAplicarDesconto, NaoPossuiChildAberta)
                .BindAction(Key.F7, AcaoEscolherTabelaPrecos, NaoPossuiChildAberta)
                .BindAction(Key.F8, AcaoVincularVendedor, NaoPossuiChildAberta)
                .BindAction(Key.F9, AcaoListarFaturamentos, NaoPossuiChildAberta)
                .BindAction(Key.F11, AcaoTrocarEmpresa, NaoPossuiChildAberta)
                .BindAction(Key.F12, AcaoIniciarNovoFaturamento, NaoPossuiChildAberta);
        }

        public static class Factory
        {
            private static FaturamentoCheckout _currentView;
            public static IFaturamentoView CurrentView => _currentView ?? CreateView();

            public static IFaturamentoView CreateView()
            {
                if (_currentView != null) return _currentView;

                _currentView = new FaturamentoCheckout(new FaturamentoCheckoutViewModel());
                _currentView.Closed += (sender, args) => _currentView = null;

                return _currentView;
            }
        }

        public FaturamentoCheckoutViewModel ViewModel { get; }

        public void ShowView()
        {
            Dispatcher.Invoke(() =>
            {
                if (IsLoaded)
                {
                    WindowState = WindowState.Maximized;
                    Keyboard.Focus(this);
                    return;
                }

                Show();
            });
        }

        private void FaturamentoCheckout_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            ViewModel.Inicializar();
        }

        private void FaturamentoCheckout_OnContentRendered(object sender, EventArgs e)
        {
            Keyboard.Focus(ControlCheckoutBox);
            AbrirPreferenciasSeNaoConfigurado();
        }

        private void ControlCheckoutBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Tab || e.Key == Key.OemBackTab)
            {
                e.Handled = true;
            }
        }
    }
}