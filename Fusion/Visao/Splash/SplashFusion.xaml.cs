using System;
using System.Windows;
using Fusion.Visao.Login;

namespace Fusion.Visao.Splash
{
    public partial class SplashFusion
    {
        private readonly SplashFusionModel _viewModel = new SplashFusionModel();

        public SplashFusion()
        {
            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.ConcluiuCarregamento += AbrirPoximaTela;
        }

        private void AbrirPoximaTela(object sender, EventArgs e)
        {
            new LoginAdm().Show();
            Close();
        }

        private async void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            await _viewModel.InicializaSistemaAsync();
        }

        private async void OnUsarConexaoDevelop(object sender, RoutedEventArgs e)
        {
            await RunTaskWithProgress(() =>
            {
                _viewModel.UsarMesmaConexaoDoDevelop();
            });

            await _viewModel.InicializaSistemaAsync();
        }
    }
}