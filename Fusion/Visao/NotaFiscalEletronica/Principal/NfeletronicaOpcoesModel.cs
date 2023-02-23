using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using DFe.Utils;
using Fusion.Sessao;
using Fusion.Visao.NfeCartaCorrecao;
using Fusion.Visao.NotaFiscalEletronica.Cancelamento;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using FusionWPF.SharedViews.AutorizarOperacao;
using ComponenteEmail = FusionCore.FusionAdm.Componentes.Email;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public sealed class NfeletronicaOpcoesModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        public ICommand CommandDanfe => GetSimpleCommand(DanfeAction);
        public ICommand CommandCancela => GetSimpleCommand(CancelaNfeAction);
        public ICommand CommandEmail => GetSimpleCommand(EnviaEmailAction);
        public ICommand CommandCartaCorrecao => GetSimpleCommand(CartaCorrecaoAction);
        public ICommand CommandDownloadXml => GetSimpleCommand(DownloadXmlAction);

        public bool NaoEstaCancelada
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public NfeletronicaOpcoesModel(Nfeletronica nfe)
        {
            _nfe = nfe;
            NaoEstaCancelada = _nfe.Cancelamento?.Status.EstaCancelado != true;
        }

        public event EventHandler JanelaCancelarFechada;

        private void OnJanelaCancelarFechada()
        {
            JanelaCancelarFechada?.Invoke(this, EventArgs.Empty);
        }

        private void DanfeAction(object predicate)
        {
            AvisoNotaDenegada();

            try
            {
                DanfeNfeHelper.GeraDanfe(_nfe);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void AvisoNotaDenegada()
        {
            if (_nfe.Finalizacao.IsDenegado)
            {
                DialogBox.MostraInformacao("NF-E está denegada");
            }
        }

        private void DownloadXmlAction(object obj)
        {
            AvisoNotaDenegada();

            var dialog = new SaveFileDialog { Filter = @"Arquivo Xml|*.xml" };
            var showDialog = dialog.ShowDialog();

            if (showDialog == DialogResult.Cancel)
                return;

            if (dialog.FileName.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não foi selecionado um caminho");
                return;
            }

            var xmlAutorizado = _nfe.Finalizacao?.XmlAutorizado;

            if (xmlAutorizado.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não encontrei XML autorizado para essa NF-e");
                return;
            }

            FuncoesXml.SalvarStringXmlParaArquivoXml(xmlAutorizado, dialog.FileName);

            DialogBox.MostraInformacao("XML foi salvo com sucesso");
            OnJanelaCancelarFechada();
        }

        private void CancelaNfeAction(object predicate)
        {
            var payloadNfe = new NfeCancelada(_nfe.Id, _nfe.TotalFinal);

            var sessao = SessaoSistema.Instancia.SessaoManager;

            var autorizarUsuario = new AutorizarUsuarioAdm(sessao);

            var autorizarCancelamento = new AutorizarOperacaoView(
                sessao,
                autorizarUsuario,
                SessaoSistema.Instancia.UsuarioLogado,
                _nfe.Id.ToString(),
                Permissao.NFE_CANCELAR,
                payloadNfe,
                () => CancelarNfe());

            autorizarCancelamento.ExecutarAcao();
        }

        private void CancelarNfe()
        {
            if (_nfe.Finalizacao.IsDenegado)
            {
                DialogBox.MostraAviso("NF-E denegada não pode ser cancelada");
                return;
            }

            var sessao = SessaoSistema.Instancia.SessaoManager;
            var emissor = _nfe.Emitente.CarregarDadosEmissor(sessao).EmissorFiscalNfe;

            if (emissor == null)
            {
                DialogBox.MostraAviso("Não foi possível encontrar o emissor desta NF-e");
                return;
            }

            var contexto = new NfeCancelamentoContexto(_nfe, emissor, SessaoSistema.Instancia.UsuarioLogado);
            var view = new NfeCancelamentoView(contexto);

            view.ShowDialog();
            OnJanelaCancelarFechada();
        }

        private void EnviaEmailAction(object predicate)
        {
            AvisoNotaDenegada();

            var pessoaId = _nfe.Destinatario.GetPessoaId();
            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = "NOTA FISCAL ELETRONICA";
            behavior.CorpoMensagem = "Segue em anexo o DANFE e o XML";

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var emails = repositorio.BuscarEmailsPelaPessoaId(pessoaId);

                behavior.Emails = new ObservableCollection<ComponenteEmail>(emails);
            }

            new EnvioEmailView(behavior).ShowDialog();
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<ComponenteEmail> e)
        {
            if (!(sender is EnvioEmailBehavior behavior))
            {
                return;
            }

            DanfeNfeHelper.EnviaEmail(_nfe, e, behavior.Assunto, behavior.CorpoMensagem);
        }

        private void CartaCorrecaoAction(object p)
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.NFE_CARTA_CORRECAO);

            if (_nfe.Finalizacao.IsDenegado)
            {
                DialogBox.MostraAviso("NF-E denegada não pode conter carta correção");
                return;
            }

            new NfeCartaCorrecaoForm(_nfe).ShowDialog();
        }
    }
}