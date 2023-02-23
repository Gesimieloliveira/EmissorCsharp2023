using System;
using System.Windows;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Emissao
{
    public partial class CteEletronicaOpcoes
    {
        private readonly CteEletronicaOpcoesModel _viewModel;

        public CteEletronicaOpcoes(Cte cte)
        {
            InitializeComponent();
            _viewModel = new CteEletronicaOpcoesModel(cte);
            DataContext = _viewModel;
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            if (_viewModel.EstaCancelada == false)
                return;

            DialogBox.MostraAviso("Opa, esta CT-e está cancelada");
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.JanelaCancelarFechada += JanelaCancelarFechadaHandler;
        }

        private void JanelaCancelarFechadaHandler(object sender, EventArgs e)
        {
            Close();
        }
    }
}