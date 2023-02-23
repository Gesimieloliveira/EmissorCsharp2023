using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public partial class EventoIncluirPagamentoForm
    {
        private readonly EventoIncluirPagamentoFormModel _model;

        public EventoIncluirPagamentoForm(EventoIncluirPagamentoFormModel model)
        {
            _model = model;
            _model.Fechar += delegate { Close(); };
            InitializeComponent();
            DataContext = model;
        }

        private void IncluirPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            var model = new IncluirPagamentoFormModel(null);
            model.IncluidoPagamento += delegate(object o, IncluirPagamentoFormModel formModel)
                {
                    _model.IncluirPagamento(formModel);
                };
            new IncluirPagamentoForm(model).ShowDialog();
        }

        private void DeletarInformacaoPagamento_Click(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente remover a informação pagamento?",
                MessageBoxImage.Question)) return;

            _model.RemoverInormacaoPagamento();
        }

        private void Salvar_OnClick(object sender, RoutedEventArgs e)
        {
            _model.SalvarEventoMdfe();
        }

        private void EventoIncluirPagamentoForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Iniciar();
        }

        private void EditarDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var model = new IncluirPagamentoFormModel(_model.InformacaoPagamentoSelecionada);
            model.IncluidoPagamento += delegate (object o, IncluirPagamentoFormModel formModel)
            {
                _model.IncluirPagamento(formModel);
            };
            new IncluirPagamentoForm(model).ShowDialog();
        }

        private void Editar_OnClick(object sender, RoutedEventArgs e)
        {
            var model = new IncluirPagamentoFormModel(_model.InformacaoPagamentoSelecionada);
            model.IncluidoPagamento += delegate (object o, IncluirPagamentoFormModel formModel)
            {
                _model.IncluirPagamento(formModel);
            };
            new IncluirPagamentoForm(model).ShowDialog();
        }
    }
}
