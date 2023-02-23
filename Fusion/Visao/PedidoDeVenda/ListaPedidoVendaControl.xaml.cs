using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class ListaPedidoVendaControl
    {
        private readonly ListaPedidoVendaControlModel _contexto;

        public ListaPedidoVendaControl(ListaPedidoVendaControlModel contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.AtualizarListagem();
            DataContext = _contexto;
        }

        private void Imprimir_OnClick(object sender, RoutedEventArgs e)
        {
            var botao = (sender) as Button;

            _contexto.ItemSelecionado = (PedidoVendaDTO) botao.Tag;
            _contexto.VisualizaPedido();
        }

        private void DoubleClickItemHandler(object sender, MouseButtonEventArgs e)
        {
            AcaoSelecionar();
        }

        private void AcaoSelecionar()
        {
            try
            {
                _contexto.SelecionarPedido();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void PKeyDownItemHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            e.Handled = true;
            AcaoSelecionar();
        }
    }
}
