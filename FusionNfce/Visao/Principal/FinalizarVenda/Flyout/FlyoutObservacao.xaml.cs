using System.Windows;

namespace FusionNfce.Visao.Principal.FinalizarVenda.Flyout
{
    public partial class FlyoutObservacao
    {
        private FlyoutObservacaoModel GetModel => (FlyoutObservacaoModel)DataContext;

        public FlyoutObservacao()
        {
            InitializeComponent();
        }

        private void OnClickBotaoAdicionaObservacao(object sender, RoutedEventArgs e)
        {
            GetModel.OnAdicionaObservacao();
        }
    }
}
