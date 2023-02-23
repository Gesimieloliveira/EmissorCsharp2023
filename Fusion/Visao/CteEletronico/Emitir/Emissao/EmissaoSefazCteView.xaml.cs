using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Emissao
{
    public partial class EmissaoSefazCteView
    {
        private readonly EmissaoSefazCteViewModel _model;

        public EmissaoSefazCteView(EmissaoSefazCteViewModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
            _model.Fechar += delegate
            {
                Close();
            };
        }

        private void EmitirCte_Clique(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.IniciarEmissao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EmissaoSefazCteView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }
    }
}
