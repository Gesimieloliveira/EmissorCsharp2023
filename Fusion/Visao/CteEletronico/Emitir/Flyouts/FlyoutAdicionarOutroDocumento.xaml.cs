using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts
{
    public partial class FlyoutAdicionarOutroDocumento
    {
        private FlyoutAdicionarOutroDocumentoModel _viewModel;

        public FlyoutAdicionarOutroDocumento()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaOutroDocumento(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnAdicionarOutroDocumento();
                ComboBoxTipoDocumento.Focus();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FlyoutAdicionarOutroDocumento_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as FlyoutAdicionarOutroDocumentoModel;
            if (model == null) return;
            _viewModel = (FlyoutAdicionarOutroDocumentoModel) DataContext;
        }
    }
}