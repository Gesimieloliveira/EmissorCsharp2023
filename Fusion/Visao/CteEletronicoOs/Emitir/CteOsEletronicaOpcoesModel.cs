using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using DFe.Utils;
using Fusion.Sessao;
using Fusion.Visao.CteEletronico.Cancelar;
using Fusion.Visao.CteEletronico.CCe;
using Fusion.Visao.CteEletronicoOs.Helpers;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionAdm.FabricaRepositorio;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;

namespace Fusion.Visao.CteEletronicoOs.Emitir
{
    public class CteOsEletronicaOpcoesModel : ViewModel
    {
        private readonly CteOs _cteOs;

        public CteOsEletronicaOpcoesModel(CteOs cteOs)
        {
            _cteOs = cteOs;
        }

        public ICommand CommandCancelar => GetSimpleCommand(CancelarAction);
        public ICommand CommandDownloadXml => GetSimpleCommand(DownloadXmlAction);
        public ICommand CommandDanfe => GetSimpleCommand(DanfeAction);
        public ICommand CommandEmail => GetSimpleCommand(EnviaEmailAction);
        public ICommand CommandCartaCorrecao => GetSimpleCommand(CartaCorrecaoAction);

        private void CartaCorrecaoAction(object obj)
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CTE_OS_CARTA_CORRECAO);

            var janela = new CteCartaCorrecao(_cteOs, new FabricaRepositorioCteOs());
            janela.ShowDialog();
        }

        private void EnviaEmailAction(object obj)
        {
            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = "CONHECIMENTO TRANSPORTE ELETRÔNICO OUTROS SERVIÇOS";
            behavior.CorpoMensagem = "Segue em anexo o DACTE e o XML";

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var emails = repositorio.BuscarEmails(_cteOs.Tomador);

                behavior.Emails = new ObservableCollection<Email>(emails);
            }

            new EnvioEmailView(behavior).ShowDialog();
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<Email> e)
        {
            var behavior = sender as EnvioEmailBehavior;

            if (behavior == null)
            {
                return;
            }

            new CTeOsImpressaoHelper().EnviaEmail(_cteOs, e, behavior.Assunto, behavior.CorpoMensagem);
        }

        private void DanfeAction(object obj)
        {
            try
            {
                var danfeHelper = new CTeOsImpressaoHelper();
                var danfe = danfeHelper.GeraDanfe(_cteOs);

                Process.Start(danfe);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void CancelarAction(object obj)
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CTE_OS_CANCELAR);

            var model = new CancelamentoCTeModel(new CancelarRn(_cteOs));
            var janela = new CancelamentoCTe(model);
            janela.ShowDialog();
            OnFechar();
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

            var xmlAutorizado = _cteOs.Emissao?.XmlAutorizado;

            if (xmlAutorizado.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não existe Xml Autorizado");
                return;
            }

            FuncoesXml.SalvarStringXmlParaArquivoXml(xmlAutorizado, dialog.FileName);

            DialogBox.MostraInformacao("Salvou Xml Autorizado com sucesso");
            OnFechar();
        }
    }
}