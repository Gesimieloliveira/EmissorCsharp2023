using System;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Emissao
{
    public partial class MdfeEletronicaOpcoes
    {
        private readonly int _mdfeId;
        private readonly MdfeEletronicaOpcoesModel _viewModel;

        public MdfeEletronicaOpcoes(int mdfeId)
        {
            _mdfeId = mdfeId;
            InitializeComponent();
            _viewModel = new MdfeEletronicaOpcoesModel();
            _viewModel.JanelaCancelarFechada += FecharJanela;
            DataContext = _viewModel;
        }

        private void FecharJanela(object sender, EventArgs e)
        {
            Dispatcher.Invoke(Close);
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            _viewModel.BuscarMdfe(_mdfeId);
            _viewModel.InicializaModel();

            if (_viewModel.EstaCancelada == false)
                return;

            DialogBox.MostraAviso("Opa, esta MDF-e está cancelada");
            Close();
        }
    }
}