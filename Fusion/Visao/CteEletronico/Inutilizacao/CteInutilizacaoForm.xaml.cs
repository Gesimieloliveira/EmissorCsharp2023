using System;

namespace Fusion.Visao.CteEletronico.Inutilizacao
{
    public partial class CteInutilizacaoForm
    {
        public CteInutilizacaoForm()
        {
            var model = new CteInutilizacaoFormModel();
            model.Fechar += Fechar;
            DataContext = model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Dispatcher.Invoke(Close);
        }
    }
}