using System.Windows;

namespace Fusion.Visao.Produto
{
    public partial class FlyoutRegraTributacao
    {
        private FlyoutRegraTributacaoModel GetModel => (FlyoutRegraTributacaoModel) DataContext;

        public FlyoutRegraTributacao()
        {
            InitializeComponent();
        }

        private void ClickSalvarRegraHandler(object sender, RoutedEventArgs e)
        {
            GetModel.SalvarRegra();
        }

        private void ClickRemoverRegraHandler(object sender, RoutedEventArgs e)
        {
            GetModel.DeletarRegra();
        }
    }
}