using System.Windows;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class ObservacaoPedidoVendaControl
    {
        private readonly ObservacaoPedidoVendaControlModel _model;

        public ObservacaoPedidoVendaControl(ObservacaoPedidoVendaControlModel model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
        }

        private void ConfirmarClickHandler(object sender, RoutedEventArgs e)
        {
            _model.ConfirmarAlteracoes();
        }
    }
}
