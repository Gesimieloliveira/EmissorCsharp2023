using System;
using System.Windows;

namespace Fusion.Visao.Lancamentos
{
    public partial class LancamentoCteEntradaForm
    {
        private readonly LancamentoCteEntradaFormModel _model;

        public LancamentoCteEntradaForm(LancamentoCteEntradaFormModel model)
        {
            _model = model;
            _model.Fechar += Fechar;
            InitializeComponent();
            DataContext = _model;
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void LancamentoCteEntradaForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.Inicializa();
        }
    }
}
