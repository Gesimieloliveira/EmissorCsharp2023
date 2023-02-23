using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using DFe.Utils;
using Fusion.Sessao;
using Fusion.Visao.CteEletronico.Cancelar;
using Fusion.Visao.CteEletronico.CCe;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Helpers;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionAdm.FabricaRepositorio;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using ComponenteEmail = FusionCore.FusionAdm.Componentes.Email;

namespace Fusion.Visao.CteEletronico.Emitir.Emissao
{
    public sealed class CteEletronicaOpcoesModel : ModelBase
    {
        private readonly Cte _cte;
        private bool _estaAutorizado;
        private bool _botaoConsultaProtocolo;
        public ICommand CommandDanfe => GetSimpleCommand(DanfeAction);
        public ICommand CommandCancela => GetSimpleCommand(CancelaNfeAction);
        public ICommand CommandEmail => GetSimpleCommand(EnviaEmailAction);
        public ICommand CommandCartaCorrecao => GetSimpleCommand(CartaCorrecaoAction);
        public ICommand CommandDownloadXml => GetSimpleCommand(DownloadXmlAction);

        public bool EstaCancelada => _cte?.Cancelamento?.StatusResposta == 135 || _cte?.Cancelamento?.StatusResposta == 136 || _cte?.Cancelamento?.StatusResposta == 134;

        public bool EstaAutorizado
        {
            get { return _estaAutorizado; }
            set
            {
                if (value == _estaAutorizado) return;
                _estaAutorizado = value;
                PropriedadeAlterada();
            }
        }

        public bool BotaoConsultaProtocolo
        {
            get { return _botaoConsultaProtocolo; }
            set
            {
                if (value == _botaoConsultaProtocolo) return;
                _botaoConsultaProtocolo = value;
                PropriedadeAlterada();
            }
        }

        public CteEletronicaOpcoesModel(Cte cte)
        {
            _cte = cte;
            EstaAutorizado = cte.CteEmissao.CodigoAutorizacao == 100;
            BotaoConsultaProtocolo = !EstaAutorizado;
        }

        public event EventHandler JanelaCancelarFechada;

        private void OnFechar()
        {
            JanelaCancelarFechada?.Invoke(this, EventArgs.Empty);
        }

        private void DanfeAction(object predicate)
        {
            try
            {
                var danfeHelper = new CTeImpressaoHelper();
                danfeHelper.GeraDanfe(_cte);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void DownloadXmlAction(object obj)
        {
            var dialog = new SaveFileDialog { Filter = @"Arquivo Xml (.xml)|*.xml" };
            dialog.ShowDialog();

            if (dialog.FileName.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não foi selecionado um caminho");
                return;
            }

            var xmlAutorizado = _cte.CteEmissao?.XmlAutorizado;

            if (xmlAutorizado.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não existe Xml Autorizado");
                return;
            }

            FuncoesXml.SalvarStringXmlParaArquivoXml(xmlAutorizado, dialog.FileName);

            DialogBox.MostraInformacao("Salvou Xml Autorizado com sucesso");
            OnFechar();
        }

        private void CancelaNfeAction(object predicate)
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CTE_CANCELAR);

            var model = new CancelamentoCTeModel(new CancelarRn(_cte));
            var janela = new CancelamentoCTe(model);
            janela.ShowDialog();
            OnFechar();
        }

        private void EnviaEmailAction(object predicate)
        {
            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = "CONHECIMENTO TRANSPORTE ELETRÔNICO";
            behavior.CorpoMensagem = "Segue em anexo o DACTE e o XML";

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var emails = repositorio.BuscarEmails(_cte.CteDestinatario.Destinatario);

                behavior.Emails = new ObservableCollection<ComponenteEmail>(emails);
            }

            new EnvioEmailView(behavior).ShowDialog();
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<ComponenteEmail> e)
        {
            var behavior = sender as EnvioEmailBehavior;

            if (behavior == null)
            {
                return;
            }

            new CTeImpressaoHelper().EnviaEmail(_cte, e, behavior.Assunto, behavior.CorpoMensagem);
        }

        private void CartaCorrecaoAction(object p)
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CTE_CARTA_CORRECAO);

            var janela = new CteCartaCorrecao(_cte, new FabricaRepositorioCte());
            janela.ShowDialog();
        }
    }
}