using System;

namespace Fusion.Visao.EmissorFiscalEletronico
{
    public partial class CertificadoDigitalForm
    {
        public CertificadoDigitalForm(CertificadoDigitalFormModel model)
        {
            model.FecharTela += FecharTela;
            DataContext = model;
            InitializeComponent();
        }

        private void FecharTela(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
