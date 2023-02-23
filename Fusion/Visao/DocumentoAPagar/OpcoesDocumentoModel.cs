using System;
using System.Windows.Input;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using Fusion.Visao.DocumentoAPagar.Lancamentos;
using Fusion.Visao.Menu;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.DocumentoAPagar
{
    public class OpcoesDocumentoModel : ViewModel
    {
        private readonly DocumentoPagar _documento;
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly Notificador _notificador;
        public ICommand ComandoEditarDocumento => GetSimpleCommand(Editardocumento);
        public ICommand ComandoEfetuarLancamento => GetSimpleCommand(EfetuarLancamento);
        public ICommand ComandoFechar => GetSimpleCommand(FecharJanela);
        public ICommand ComandoQuitarDocumento => GetSimpleCommand(QuitarDocumento);
        public ICommand ComandoRecibo => GetSimpleCommand(ImprimirDocumentoAction);

        public OpcoesDocumentoModel(DocumentoPagar documento, Notificador notificador)
        {
            _documento = documento;
            _notificador = notificador;
            EstaQuitado = _documento.EstaQuitado();
        }

        public event EventHandler<DocumentoPagar> ImprimirDocumento;

        public bool EstaQuitado
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        private void ImprimirDocumentoAction(object obj)
        {
            if (_documento.NaoEstaQuitado())
            {
                DialogBox.MostraInformacao("Documento não esta quitado");
                return;
            }

            OnImprimirDocumento(_documento);
            OnFechar();
        }

        private void QuitarDocumento(object obj)
        {
            _sessaoSistema
                .UsuarioLogado
                .VerificaPermissao
                .IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_QUITAR);

            ControleCaixaGestorFacade
                .ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);

            var valorRestante = (_documento.ValorAjustado - _documento.ValorQuitado);
            var msg = $"Deseja quitar este documento?\n Valor Documento: {_documento.ValorAjustado:C} \n Valor Restante: {valorRestante:C}";
            if (!DialogBox.MostraDialogoDeConfirmacao(msg))
            {
                return;
            }

            var servico = new ServicoPagarDocumento(_sessaoSistema.SessaoManager)
            {
                Historico = "QUITADO POR AÇÃO RÁPIDA NAS OPÇÕES DO DOCUMENTO",
                DocumentoId = _documento.Id,
                MarcarComoQuitado = true,
                Usuario = _sessaoSistema.UsuarioLogado,
                ValorPagamento = _documento.ValorEmAberto
            };

            servico.FazerPagamento();
            _notificador.Notificar("documentoPagarQuitado", new NotificacaoArgs(_documento));

            OnFechar();
        }

        private void Editardocumento(object obj)
        {
            var usuarioLogado = _sessaoSistema.UsuarioLogado;
            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_VISUALIZAR);

            RefreshDocumento();

            var model = new DocumentoPagarFormModel(_documento, _notificador);
            var dialog = new DocumentoPagarForm(model);

            MenuPrincipal.Instancia.ShowChildWindowAsync(dialog);
            OnFechar();
        }

        private void RefreshDocumento()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);
                repositorio.Refresh(_documento);
            }
        }

        private void EfetuarLancamento(object obj)
        {
            RefreshDocumento();
            OnFechar();

            var model = new DocumentoAPagarLancamentoModel(_documento, _notificador);
            var dialog = new DocumentoAPagarLancamento(model);

            dialog.ShowDialog();
        }

        private void FecharJanela(object obj)
        {
            OnFechar();
        }

        protected virtual void OnImprimirDocumento(DocumentoPagar e)
        {
            ImprimirDocumento?.Invoke(this, e);
        }
    }
}