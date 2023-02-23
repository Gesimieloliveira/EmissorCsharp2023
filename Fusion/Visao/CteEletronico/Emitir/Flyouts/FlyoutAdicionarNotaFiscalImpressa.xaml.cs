using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts
{
    public partial class FlyoutAdicionarNotaFiscalImpressa
    {
        private FlyoutAdicionarNotaFiscalImpressaModel _viewModel;

        public FlyoutAdicionarNotaFiscalImpressa()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaNotaFiscalImpressa(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnAdicionarNotaFiscalImpressa();
                TextBoxSerie.Focus();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FlyoutAdicionarNotaFiscalImpressa_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as FlyoutAdicionarNotaFiscalImpressaModel;
            if (model == null) return;
            _viewModel = (FlyoutAdicionarNotaFiscalImpressaModel) DataContext;
        }
    }
}