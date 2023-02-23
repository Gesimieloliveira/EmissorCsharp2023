using System;
using System.Windows;
using FusionCore.FusionAdm.Financeiro;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.TipoDocumentoFinanceiro
{
    public partial class TipoDocumentoFinanceiroForm
    {
        private readonly TipoDocumentoFinanceiroFormModel _model;

        public TipoDocumentoFinanceiroForm(TipoDocumento tipoDocumento)
        {
            _model = new TipoDocumentoFinanceiroFormModel(tipoDocumento);
            DataContext = _model;
            InitializeComponent();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Salvar();
                DialogBox.MostraMensagemSalvouComSucesso();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Deletar();
                DialogBox.MostraMensagemDeletouComSucesso();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
