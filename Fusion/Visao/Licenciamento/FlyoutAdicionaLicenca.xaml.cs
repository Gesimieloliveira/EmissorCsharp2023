using System.Windows;

namespace Fusion.Visao.Licenciamento
{
    public partial class FlyoutAdicionaLicenca
    {
        public FlyoutAdicionaLicencaModel GetModel => (FlyoutAdicionaLicencaModel) DataContext;

        public FlyoutAdicionaLicenca()
        {
            InitializeComponent();
        }

        private void ClickAtivarHandler(object sender, RoutedEventArgs e)
        {
            GetModel.ClickAtivarHandler();
        }
    }
}