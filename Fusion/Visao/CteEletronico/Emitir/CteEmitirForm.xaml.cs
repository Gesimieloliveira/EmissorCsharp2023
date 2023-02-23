using System;
using System.Windows;
using System.Windows.Controls;
using FusionCore.Helpers.Binding;

namespace Fusion.Visao.CteEletronico.Emitir
{
    public partial class CteEmitirForm
    {
        private bool _eventoJaRegistrado;
        private readonly CteEmitirFormModel _model;

        public CteEmitirForm(CteEmitirFormModel model)
        {
            DataContext = model;
            model.FecharTela += FecharTela;
            _model = model;
            InitializeComponent();            
        }

        private CteEmitirFormModel ViewModel => DataContext as CteEmitirFormModel;

        private void CteEmitirForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (TabControl != null && _eventoJaRegistrado == false)
            {
                TabControl.SelectionChanged += Selector_OnSelectionChanged;
                _eventoJaRegistrado = true;
            }

            _model.VerificaSeTemEmissaoPendente();
        }

        private void FecharTela(object sender, EventArgs e)
        {
            Close();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingHelper.ForceFocusedElementUpdateSource();

            ViewModel.CarregarInformacaoPadraoAba();

            if (e.OriginalSource is TabControl)
            {
                ViewModel.SalvarCteAba();
            }
        }
    }
}