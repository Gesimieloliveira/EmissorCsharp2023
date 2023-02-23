using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.PedidoDeVenda.Servicos;
using Fusion.Visao.Vendas.FaturamentoCheckout;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.PedidoDeVenda.Lista
{
    public partial class OpcoesPedidoVenda
    {
        private readonly OpcoesPedidoVendaModel _contexto;
        private readonly PedidoVendaController _pedidoVendaController;

        public OpcoesPedidoVenda(PedidoVendaDTO pedido, PedidoVendaController pedidoVendaController)
        {
            _contexto = new OpcoesPedidoVendaModel(pedido);

            _contexto.OnConverteu += (s, e) =>
            {
                Close();
                var view = FaturamentoCheckout.Factory.CreateView();
                view.ViewModel.CarregarComFaturamento(e.Id);
                view.ShowView();
            };

            _pedidoVendaController = pedidoVendaController;

            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
        }

        private void EditarDocumentoClickHandler(object sender, RoutedEventArgs e)
        {
            Close();

            _pedidoVendaController.AbrirFormularioEdicao(_contexto.PedidoSelecionado);
        }

        private void ImprimirClickHandler(object sender, RoutedEventArgs e)
        {
            Close();

            var impressor = new ImpressorPedidoVenda(new SessaoManagerAdm());

            impressor.Visualizar(_contexto.PedidoSelecionado.Id);
        }

        private void EnviarPorEmailClickHandler(object sender, RoutedEventArgs e)
        {
            Close();

            _contexto.EnviarPorEmail();
        }
    }
}