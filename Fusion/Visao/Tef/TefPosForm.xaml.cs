using System;

namespace Fusion.Visao.Tef
{
    public partial class TefPosForm
    {
        private readonly TefPosFormModel _model;

        public TefPosForm(TefPosFormModel model)
        {
            _model = model;
            _model.Fechar += Fechar;
            DataContext = _model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void TefPosForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Load();
        }
    }
}
