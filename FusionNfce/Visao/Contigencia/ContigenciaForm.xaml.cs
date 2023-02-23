using System;

namespace FusionNfce.Visao.Contigencia
{
    public partial class ContigenciaForm
    {
        public ContigenciaForm(ContigenciaFormModel model)
        {
            model.FecharWindow += FecharTelaContingencia;
            DataContext = model;
            InitializeComponent();
        }

        private void FecharTelaContingencia(object sender, EventArgs e)
        {
            Close();
        }
    }
}
