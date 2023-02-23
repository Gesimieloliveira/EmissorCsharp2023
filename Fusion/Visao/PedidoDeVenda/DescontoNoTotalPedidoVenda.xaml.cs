using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class DescontoNoTotalPedidoVenda
    {
        private readonly DescontoNoTotalPedidoVendaModel _contexto;

        public DescontoNoTotalPedidoVenda(DescontoNoTotalPedidoVendaModel contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
        }

        private void AplicarDescontoClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.AplicarDesconto();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
