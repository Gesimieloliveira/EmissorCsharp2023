using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PedidoDeVenda.Preferencias
{
    public partial class PedidoVendaPreferenciaForm
    {
        private readonly PedidoVendaPreferenciaFormModel _model;

        public PedidoVendaPreferenciaForm()
        {
            InitializeComponent();
            _model = new PedidoVendaPreferenciaFormModel();
            _model.Fechar += delegate { Close(); };
            DataContext = _model;
        }

        private void ClickSalvarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Salvar();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void PedidoVendaPreferenciaForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.Inicializa();
        }
    }
}
