using System;

namespace Fusion.Visao.CteEletronicoOs.Emitir
{
    public partial class EmissaoSefazCteOsView
    {
        private readonly EmissaoSefazCteOsViewModel _model;

        public EmissaoSefazCteOsView(EmissaoSefazCteOsViewModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }

        private void EmissaoSefazCteOsView_OnContentRendered(object sender, EventArgs e)
        {
            _model.ContentRendered();
        }
    }
}
