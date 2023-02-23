using System;

namespace FusionNfce.Visao.Principal.AvancaNumeracao
{
    public partial class AvancaNumeracaoFiscal
    {
        public AvancaNumeracaoFiscal(AvancaNumeracaoFiscalModel model)
        {
            model.Fechar += Fechar;
            InitializeComponent();
            DataContext = model;
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }
    }
}
