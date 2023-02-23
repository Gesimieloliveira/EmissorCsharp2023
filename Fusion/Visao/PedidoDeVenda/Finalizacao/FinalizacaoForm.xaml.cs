using System.Windows;
using System.Windows.Input;
using Fusion.Visao.PedidoDeVenda.Servicos;
using FusionCore.Sessao;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao
{
    public partial class FinalizacaoForm
    {
        private readonly FinalizacaoFormModel _model;

        public FinalizacaoForm(FinalizacaoFormModel model)
        {
            model.Fechar += delegate { Close(); };
            InitializeComponent();
            DataContext = model;
            _model = model;
        }

        private void Imprimir_OnClick(object sender, RoutedEventArgs e)
        {
            var impressor = new ImpressorPedidoVenda(new SessaoManagerAdm());

            impressor.Visualizar(_model.PedidoVenda.Id);
        }

        private void MarcarComoAberto_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AbrirPedido();
        }

        private void MarcarComoFinalizado_OnClick(object sender, RoutedEventArgs e)
        {
            _model.FinalizacaoPedido();
        }

        private void PKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                e.Handled = true;
                MarcarComoFinalizado_OnClick(sender, e);
            }

            if (e.Key == Key.F3)
            {
                e.Handled = true;
                MarcarComoAberto_OnClick(sender, e);
            }

            if (e.Key == Key.F4)
            {
                e.Handled = true;
                Imprimir_OnClick(sender, e);
            }
        }
    }
}
