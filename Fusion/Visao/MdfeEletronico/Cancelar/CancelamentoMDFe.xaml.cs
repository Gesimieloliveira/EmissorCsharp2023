using System;
using System.Windows;

namespace Fusion.Visao.MdfeEletronico.Cancelar
{
    public partial class CancelamentoMDFe
    {
        private readonly CancelamentoMDFeModel _model;

        public CancelamentoMDFe(CancelamentoMDFeModel model)
        {
            _model = model;
            model.FecharTela += FecharTela;
            InitializeComponent();
            DataContext = _model;
        }

        private void FecharTela(object sender, EventArgs e)
        {
            Dispatcher.Invoke(Close);
        }

        private void OnClickCancelar(object sender, RoutedEventArgs e)
        {
            _model.CancelarAsync();
        }
    }
}