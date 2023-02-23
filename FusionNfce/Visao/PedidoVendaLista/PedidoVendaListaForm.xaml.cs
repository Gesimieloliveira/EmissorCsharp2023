using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.PedidoVendaLista
{
    public partial class PedidoVendaListaForm
    {
        private PedidoVendaListaFormModel _model;

        public PedidoVendaListaForm(PedidoVendaListaFormModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = model;
        }

        private void Imprimir_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SetPedidoModel(sender);
                _model.ImprimirPedido();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void ConverterPedidoEmNfce_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Continuar com a Importação do Pedido para a NFC-e?",
                MessageBoxImage.Question))
                return;

            SetPedidoModel(sender);

            ConvertePedidoParaNfce();
        }

        private void ConvertePedidoParaNfce()
        {
            try
            {
                _model.ConvertePedidoParaNfce();
                Close();
            }
            catch (RegraNegocioException ex)
            {
                DialogBox.MostraAviso(ex.Message, ex.Detalhes);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void SetPedidoModel(object sender)
        {
            var button = (Button)sender;
            _model.ItemSelecionado = (PedidoVendaDTO)button.Tag;
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Continuar com a Importação do Pedido para a NFC-e?",
                MessageBoxImage.Question))
                return;

            var listBoxItem = (ListBoxItem)sender;
            _model.ItemSelecionado = (PedidoVendaDTO)listBoxItem.DataContext;

            ConvertePedidoParaNfce();
        }
    }
}
