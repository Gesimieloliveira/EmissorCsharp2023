using System;

namespace FusionNfce.Visao.Principal
{
    public partial class MaisOpcoesForm
    {
        public MaisOpcoesForm(MaisOpcoesFormModel model)
        {
            model.Fechar += Fechar;
            DataContext = model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }
    }
}
