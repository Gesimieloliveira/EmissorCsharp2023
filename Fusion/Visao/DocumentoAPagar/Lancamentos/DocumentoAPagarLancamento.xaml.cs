using System;
using System.Windows;
using Fusion.Sessao;
using FusionCore.Papeis.Enums;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos
{
    public partial class DocumentoAPagarLancamento
    {
        private readonly DocumentoAPagarLancamentoModel _model;

        public DocumentoAPagarLancamento(DocumentoAPagarLancamentoModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }

        private void AdicionaValor_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AbrirFlyoutAdicionaValor();
        }

        private void EstornaItemOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SessaoSistema
                    .Instancia
                    .UsuarioLogado
                    .VerificaPermissao
                    .IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ESTORNAR_LANCAMENTO);

                if (!DialogBox.MostraDialogoDeConfirmacao("Deseja realmente estornar este lançamento?"))
                {
                    return;
                }

                _model.EstornarItem();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AdicionaDesconto_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AbrirFlyoutAdicionaDesconto();
        }

        private void AdicionaJuro_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AbrirFlyoutAdicionaJuro();
        }
    }
}
