using System;

namespace FusionNfce.Visao.Principal.FinalizarVenda.Tef.Pos
{
    public partial class EfetuaPagamentoPosForm
    {
        public EfetuaPagamentoPosForm(EfetuaPagamentoPosFormModel model)
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
