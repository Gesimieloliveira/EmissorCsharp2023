using System.Windows;
using System.Windows.Input;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class TipoPedidoControl
    {
        private readonly TipoPedidoContexto _contexto;

        public TipoPedidoControl(TipoPedidoContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void RootPreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                e.Handled = true;
                AcaoEscolherPedido();
                return;
            }

            if (e.Key == Key.F3)
            {
                e.Handled = true;
                AcaoEscolherOrcamento();
                return;
            }
        }

        private void AcaoEscolherPedido()
        {
            _contexto.EscolherPedido();
            Close(true);
        }

        private void AcaoEscolherOrcamento()
        {
            _contexto.EscolherOrcamento();
            Close(true);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
        }

        private void PedidoVendaClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolherPedido();
        }

        private void OrcamentoClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolherOrcamento();
        }
    }
}
