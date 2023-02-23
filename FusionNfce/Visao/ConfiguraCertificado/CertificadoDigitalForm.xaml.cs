using System;

namespace FusionNfce.Visao.ConfiguraCertificado
{
    public partial class CertificadoDigitalForm
    {
        private readonly CertificadoDigitalFormModel _model;

        public CertificadoDigitalForm(CertificadoDigitalFormModel certificadoDigitalFormModel)
        {
            InitializeComponent();
            _model = certificadoDigitalFormModel;
            _model.Fechar += (sender, args) => { Close(); };
            DataContext = _model;
        }

        private void CertificadoDigitalForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Inicializa();
        }
    }
}
