using System;
using System.Windows;
using FusionNfce.Visao.Login;

namespace FusionNfce.Visao.Splash
{
    public partial class SplashNfce
    {
        private readonly SplashNfceModel _viewModel = new SplashNfceModel();

        public SplashNfce()
        {
            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.ConcluiuCarregamento += ConcluiCarregamentoHandler;
        }

        private async void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            await _viewModel.InicializaSistemaAsync();
        }

        private void ConcluiCarregamentoHandler(object sender, EventArgs e)
        {
            var login = new LoginForm();
            login.Show();

            Close();
        }
    }
}