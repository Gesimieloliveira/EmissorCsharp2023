using System;

namespace FusionNfce.Visao.Configuracao.Impressao
{
    public partial class ConfiguracaoImpressaoDiretaForm
    {
        public ConfiguracaoImpressaoDiretaForm()
        {

            var model = new ConfiguracaoImpressaoDiretaFormModel();
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
