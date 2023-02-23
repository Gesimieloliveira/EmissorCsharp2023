using System;
using System.Windows;

namespace Fusion.Visao.Licenciamento
{
    public partial class PainelLicencas
    {
        private PainelLicencasModel GetModel => (PainelLicencasModel) DataContext;

        public PainelLicencas()
        {
            DataContext = new PainelLicencasModel();
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            GetModel?.Carregar();
            GetModel?.IniciarMonitoramento();
        }

        private void ClickAdicionarLicencaHandler(object sender, RoutedEventArgs e)
        {
            GetModel?.AdicionarLicencaHandler();
        }

        protected override void OnClosed(EventArgs e)
        {
            GetModel?.PararMonitoramento();
        }

        private void ClickRemoveAcessoHandler(object sender, RoutedEventArgs e)
        {
            GetModel?.DesalocarAcessoSelecionado();
        }

        private void ClickRemoverLicencaHandler(object sender, RoutedEventArgs e)
        {
            GetModel?.DesalocarLicencaSelecionada();
        }
    }
}