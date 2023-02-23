using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts
{
    public partial class FlyoutAdicionarNfe
    {
        private FlyoutAdicionarNfeModel _viewModel;

        public FlyoutAdicionarNfe()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaNfe(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnAdicionaDocumentoNfe();
                TextBoxChaveNfe.Focus();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FlyoutAdicionarNfe_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as FlyoutAdicionarNfeModel;
            if (model == null) return;
            _viewModel = (FlyoutAdicionarNfeModel) DataContext;
        }

        private void OnClickBotaoImportarNFe(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.ImportarNFe();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}