using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Papeis.Enums;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar
{
    public partial class DocumentoPagarForm
    {
        private readonly DocumentoPagarFormModel _model;

        public DocumentoPagarForm(DocumentoPagarFormModel model)
        {
            _model = model;
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ChecarPermissoes();

            _model.PropertyChanged -= OnPropertyChanged;
            _model.PropertyChanged += OnPropertyChanged;
            _model.AtualizarModel();

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            DataContext = _model;
        }

        private void ChecarPermissoes()
        {
            var podeEstornar = SessaoSistema.Instancia.UsuarioLogado.VerificaPermissao.IsTemPermissao(
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ESTORNAR
            );

            var podeInserir = SessaoSistema.Instancia.UsuarioLogado.VerificaPermissao.IsQualquerPermissao(
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_GERAR_AVULSO,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ALTERAR
            );

            BotaoOk.Visibility = podeInserir ? Visibility.Visible : Visibility.Collapsed;
            BotaoEstornar.Visibility = podeEstornar ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_model.Situacao))
            {
                RootContent.IsEnabled = _model.Situacao != Situacao.Cancelado;
            }
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.SalvarRegistro();
                DialogBox.MostraMensagemSalvouComSucesso();

                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickEstornar(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraDialogoDeConfirmacao("Deseja realmente cancelar este documento?"))
            {
                return;
            }

            try
            {
                _model.EstornarDocumento();
                DialogBox.MostraInformacao("Documento foi estornado com sucesso!");

                Close(true);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}