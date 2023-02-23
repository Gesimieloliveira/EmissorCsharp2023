using System;

namespace FusionNfce.Visao.Principal.Tef
{
    public partial class ConfiguraTefForm 
    {
        public ConfiguraTefForm()
        {
            var model = new ConfiguraTefFormModel();
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
