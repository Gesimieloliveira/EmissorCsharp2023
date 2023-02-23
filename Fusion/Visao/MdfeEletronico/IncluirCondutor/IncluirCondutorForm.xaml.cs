using System;
using System.Windows;
using FusionCore.FusionAdm.MdfeEletronico;

namespace Fusion.Visao.MdfeEletronico.IncluirCondutor
{
    public partial class IncluirCondutorForm
    {
        private readonly IncluirCondutorFormModel _model;

        public IncluirCondutorForm(MDFeEletronico mdfe)
        {
            _model = new IncluirCondutorFormModel(mdfe);
            _model.JanelaCancelarFechada += Fechar;
            DataContext = _model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Dispatcher.Invoke(Close);
        }

        private void OnClickIncluirCondutor(object sender, RoutedEventArgs e)
        {
            _model.IncluirCondutor();
        }
    }
}
