using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts
{
    public partial class FlyoutInformacaoCarga
    {
        private FlyoutInformacaoCargaModel _viewModel;

        public FlyoutInformacaoCarga()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaInformacaoCarga(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnAdicionarInformacaoCarga();
                ComboBoxUnidadeMedida.Focus();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FlyoutInformacaoCarga_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as FlyoutInformacaoCargaModel;
            if (model == null) return;
            _viewModel = (FlyoutInformacaoCargaModel) DataContext;
        }
    }
}