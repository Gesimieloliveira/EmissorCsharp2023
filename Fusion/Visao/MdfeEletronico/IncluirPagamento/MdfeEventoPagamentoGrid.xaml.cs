using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionWPF.Base.Utils.Dialogs;
using InvalidOperationException = System.InvalidOperationException;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public partial class MdfeEventoPagamentoGrid
    {
        private readonly MdfeEventoPagamentoGridModel _model;

        public MdfeEventoPagamentoGrid(MdfeEventoPagamentoGridModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = model;
        }

        private void IncluirPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            if (_model.Eventos.Count != 0)
            {
                DialogBox.MostraAviso("Somente pode ter um único evento.");
                return;
            }

            var model = new EventoIncluirPagamentoFormModel(_model.ObterMdfe(), null);

            new EventoIncluirPagamentoForm(model).ShowDialog();

            _model.CarregarEventosPagamento();
        }

        private void MdfeEventoPagamentoGrid_OnContentRendered(object sender, EventArgs e)
        {
            _model.CarregarEventosPagamento();
        }

        private void Editar_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            var eventoPagamento = button.Tag as MDFeEventoPagamento;

            var model = new EventoIncluirPagamentoFormModel(_model.ObterMdfe(), eventoPagamento);

            new EventoIncluirPagamentoForm(model).ShowDialog();

            _model.CarregarEventosPagamento();
        }

        private void Deletar_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente deletar o evento de pagamento?",
                MessageBoxImage.Question)) return;

            var button = sender as Button;
            var eventoPagamento = button.Tag as MDFeEventoPagamento;

            _model.DeletarEventoPagamento(eventoPagamento);
        }

        private void EditarDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var datagridRow = sender as DataGridRow;
            var eventoSelecionado = datagridRow.DataContext as MDFeEventoPagamento;

            var model = new EventoIncluirPagamentoFormModel(_model.ObterMdfe(), eventoSelecionado);

            new EventoIncluirPagamentoForm(model).ShowDialog();

            _model.CarregarEventosPagamento();
        }

        private void Enviar_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente enviar o evento?", MessageBoxImage.Question)) return;

            var button = sender as Button;
            var eventoPagamento = button.Tag as MDFeEventoPagamento;

            try
            {
                _model.EnviarEventoParaSefaz(eventoPagamento);

                DialogBox.MostraInformacao("Evento vinculado ao mdf-e com sucesso!!!!");

                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
