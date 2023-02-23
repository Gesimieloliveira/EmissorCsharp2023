using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts
{
    public partial class FlyoutAddVeiculoParaTransporte
    {
        private FlyoutAddVeiculoParaTransporteModel _viewModel;

        public FlyoutAddVeiculoParaTransporte()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaVeiculoTransporte(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnAdicionaVeiculoParaTransporte();
                TextBoxCor.Focus();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FlyoutAddVeiculoParaTransporte_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as FlyoutAddVeiculoParaTransporteModel;
            if (model == null) return;
            _viewModel = (FlyoutAddVeiculoParaTransporteModel) DataContext;
        }
    }
}