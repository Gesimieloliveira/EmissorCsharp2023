using System.Windows;
using System.Windows.Input;
using Fusion.Helpers;

namespace Fusion.Visao.PedidoDeVenda.Lista
{
    public partial class GridPedidoVenda
    {
        private readonly GridPedidoVendaModel _model;
        private readonly PedidoVendaController _pedidoVendaController;

        public GridPedidoVenda(GridPedidoVendaModel model, PedidoVendaController pedidoVendaController)
        {
            _model = model;
            _pedidoVendaController = pedidoVendaController;
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
            DataContext = _model;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _model.AtualizarLista();
        }

        private void AplicarBuscaClickHandler(object sender, RoutedEventArgs e)
        {
            _model.AtualizarLista();
        }

        private void DoubleClickRowHandler(object sender, MouseButtonEventArgs e)
        {
            _pedidoVendaController.AbrirJanelaOpcoes(_model.Selecionado);
            _model.AtualizarLista();
        }

        private void ClickNovoHandler(object sender, RoutedEventArgs e)
        {
            _pedidoVendaController.AbrirFormulario();
        }
    }
}
