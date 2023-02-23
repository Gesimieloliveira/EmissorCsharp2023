using System;
using System.Windows;

namespace Fusion.Visao.Lancamentos
{
    public partial class LancamentoOutroForm
    {
        private readonly LancamentoOutroFormModel _model;

        public LancamentoOutroForm(LancamentoOutroFormModel model)
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

        private void LancamentoOutroForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }
    }
}
