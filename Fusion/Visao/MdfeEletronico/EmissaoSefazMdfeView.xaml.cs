using System;
using FusionCore.FusionAdm.MdfeEletronico;

namespace Fusion.Visao.MdfeEletronico
{
    public partial class EmissaoSefazMdfeView
    {
        private readonly EmissaoSefazMdfeViewModel _model;
        private readonly MDFeEletronico _mdfe;

        public EmissaoSefazMdfeView(EmissaoSefazMdfeViewModel model, MDFeEletronico mdfe)
        {
            InitializeComponent();
            _model = model;
            _mdfe = mdfe;
            _model.Fechar += delegate(object sender, EventArgs args) { Close(); };
            DataContext = model;
        }

        private async void EmissaoSefazMdfeView_OnContentRendered(object sender, EventArgs e)
        {
            await RunTaskWithProgress(() =>
             {
                 _model.AtualizarInformacoesMDFe(_mdfe);
             });
            _model.ContentRendered();
        }
    }
}
