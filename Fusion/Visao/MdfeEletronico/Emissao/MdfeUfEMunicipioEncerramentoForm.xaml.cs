using System;
using System.Windows;
using System.Windows.Controls;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Emissao
{
    public partial class MdfeUfEMunicipioEncerramentoForm
    {
        private readonly MdfeUfEMunicipioEncerramentoFormModel _modelo;

        public MdfeUfEMunicipioEncerramentoForm(MdfeUfEMunicipioEncerramentoFormModel modelo)
        {
            _modelo = modelo;
            InitializeComponent();
            DataContext = _modelo;
            _modelo.Fechar += delegate { Close(); };
        }

        private void CarregarCidadesComBaseNoEstado(object sender, SelectionChangedEventArgs e)
        {
            _modelo.CarregarCidadesComBaseNoEstado();
        }

        private void MdfeUfEMunicipioEncerramentoForm_OnContentRendered(object sender, EventArgs e)
        {
            _modelo.InicializaModel();
        }

        private void EnviarEncerramento_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _modelo.EnviarEncerramento();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
