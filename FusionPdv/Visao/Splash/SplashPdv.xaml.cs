using System;
using System.Windows;
using FusionPdv.Visao.Principal;

namespace FusionPdv.Visao.Splash
{
    public partial class SplashPdv
    {
        private readonly SplashPdvModel _viewModel = new SplashPdvModel();

        public SplashPdv()
        {
            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.ConcluiuCarregamento += AbrirPoximaTela;
        }

        private void AbrirPoximaTela(object sender, EventArgs e)
        {
            new Login().Show();
            Close();
        }

        private async void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            await _viewModel.InicializaSistemaAsync();
        }
    }
}