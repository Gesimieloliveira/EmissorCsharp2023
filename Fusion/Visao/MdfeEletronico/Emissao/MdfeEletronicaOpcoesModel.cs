using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using DFe.Utils;
using Fusion.Sessao;
using Fusion.Visao.MdfeEletronico.Cancelar;
using Fusion.Visao.MdfeEletronico.IncluirCondutor;
using Fusion.Visao.MdfeEletronico.IncluirPagamento;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Extencoes;
using FusionCore.FusionAdm.MdfeEletronico.Factory;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Helpers;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using MDFe.Classes.Informacoes.Evento.Flags;
using MDFe.Classes.Retorno.MDFeEvento;
using MDFe.Classes.Retorno.MDFeRetRecepcao;
using MDFe.Servicos.ConsultaProtocoloMDFe;
using MDFe.Servicos.EventosMDFe;
using MDFe.Servicos.RetRecepcaoMDFe;
using Application = System.Windows.Application;
using MDFeEvento = FusionCore.FusionAdm.MdfeEletronico.MDFeEvento;
using MDFeZeus = MDFe.Classes.Informacoes.MDFe;
using MDFe.Classes.Extencoes;

namespace Fusion.Visao.MdfeEletronico.Emissao
{
    public sealed class MdfeEletronicaOpcoesModel : ModelBase
    {
        private MDFeEletronico _mdfe;
        private bool _estaAutorizado;
        private bool _botaoConsultaProtocolo;
        public ICommand CommandDanfe => GetSimpleCommand(DanfeAction);
        public ICommand CommandCancela => GetSimpleCommand(CancelaMdfeAction);
        public ICommand CommandIncluirCondutor => GetSimpleCommand(IncluirCondutorAction);
        public ICommand CommandEncerrar => GetSimpleCommand(EncerrarAction);
        public ICommand CommandConsultaProcessamento => GetSimpleCommand(ConsultaProcessamentoAction);
        public ICommand CommandDownloadXml => GetSimpleCommand(DownloadXmlAction);
        public ICommand CommandIncluirPagamento => GetSimpleCommand(AcaoIncluirPagamento);
        public ICommand CommandEmail => GetSimpleCommand(AcaoEnviarEmail);

        public bool EstaCancelada => _mdfe.Status == MDFeStatus.Cancelada;

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

        public void InicializaModel()
        {
            EstaAutorizado = _mdfe.Status == MDFeStatus.Autorizado || _mdfe.Status == MDFeStatus.Encerrada;
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
                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(_mdfe.EmissorFiscal.EmissorFiscalMdfe);
                var danfeHelper = new MDFeImpressaoHelper();

                danfeHelper.ImprimirDanfe(_mdfe);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void CancelaMdfeAction(object predicate)
        {
            SessaoSistema.ObterUsuarioLogado().VerificaPermissao.IsTemPermissaoThrow(Permissao.MDFE_CANCELAR);

            if (ValidarCancelar()) return;

            var model = new CancelamentoMDFeModel(_mdfe);
            var janela = new CancelamentoMDFe(model);
            janela.ShowDialog();
            OnFechar();
        }

        private bool ValidarCancelar()
        {
            if (_mdfe.Status == MDFeStatus.ConsultaProcessamento)
            {
                DialogBox.MostraInformacao("MDF-e não autorizado");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.Cancelada)
            {
                DialogBox.MostraInformacao("MDF-e cancelada");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.Encerrada)
            {
                DialogBox.MostraInformacao("MDF-e encerrada");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.EmDigitacao)
            {
                DialogBox.MostraInformacao("MDF-e em digitação");
                return true;
            }
            return false;
        }

        private void IncluirCondutorAction(object predicate)
        {
            SessaoSistema.ObterUsuarioLogado().VerificaPermissao.IsTemPermissaoThrow(Permissao.MDFE_INCLUIR_CONDUTOR);

            if (ValidaIncuirCondutor()) return;

            new IncluirCondutorForm(_mdfe).ShowDialog();

            OnFechar();
        }

        private void AcaoIncluirPagamento(object obj)
        {
            SessaoSistema.ObterUsuarioLogado().VerificaPermissao.IsTemPermissaoThrow(Permissao.MDFE_INCLUIR_PAGAMENTO);

            if (_mdfe.EventosPagamentos.Any(x => x.Autorizado))
            {
                DialogBox.MostraAviso("Evento já está incluso no mdf-e");
                return;
            }

            var model = new MdfeEventoPagamentoGridModel(_mdfe);
            new MdfeEventoPagamentoGrid(model).ShowDialog();


            OnFechar();
        }

        private bool ValidaIncuirCondutor()
        {
            if (_mdfe.Status == MDFeStatus.Cancelada)
            {
                DialogBox.MostraInformacao("MDF-e está cancelada");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.ConsultaProcessamento)
            {
                DialogBox.MostraInformacao("MDF-e aguardando para ser autorizada, consulte o processamento (recibo)");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.EmDigitacao)
            {
                DialogBox.MostraInformacao("MDF-e não foi autorizada, está em Digitação (edição)");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.Encerrada)
            {
                DialogBox.MostraInformacao("MDF-e está encerrada");
                return true;
            }
            return false;
        }

        private void EncerrarAction(object p)
        {
            var usuarioLogado = ObterUsuarioLogado();

            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.MDFE_ENCERRAR);

            const string texto = "Não é possível desfazer a ação de encerramento. Deseja continuar?";

            if (DialogBox.MostraConfirmacao(texto) != MessageBoxResult.Yes)
            {
                return;
            }


            if (ValidaEncerrarMdfe()) return;

            var mdfe = MDFeZeus.LoadXmlString(_mdfe.Emissao.XmlAssinado);

            var localidadeServico = LocalidadesServico.GetInstancia();
            var estadoDto = localidadeServico.GetEstado(x => x.CodigoIbge == (int)mdfe.InfMDFe.Ide.UFFim);
            var cidadeDto = localidadeServico.GetCidade(x =>
                x.CodigoIbge == int.Parse(mdfe.InfMDFe.InfDoc.InfMunDescarga.LastOrDefault().CMunDescarga));


            var modeloUfMunicipioEncerramento = new MdfeUfEMunicipioEncerramentoFormModel();
            modeloUfMunicipioEncerramento.AdicionarUfEMunicipioEncerramentoPadrao(estadoDto, cidadeDto);

            modeloUfMunicipioEncerramento.EnviarEncerramentoManipulador += async (sender, model) =>
            {
                await Task.Run(() => EncerrarMdfe(model.Estado, model.Cidade));
            };

            new MdfeUfEMunicipioEncerramentoForm(modeloUfMunicipioEncerramento).ShowDialog();
        }

        private static UsuarioDTO ObterUsuarioLogado()
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            return usuarioLogado;
        }

        private void EncerrarMdfe(EstadoDTO estado, CidadeDTO cidade)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);


                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(_mdfe.EmissorFiscal.EmissorFiscalMdfe);
                var evento = new ServicoMDFeEvento();

                var mdfe = MDFeZeus.LoadXmlString(_mdfe.Emissao.XmlAssinado);

                var resposta = evento.MDFeEventoEncerramentoMDFeEventoEncerramento(mdfe, estado.ToZeusMdfe(), cidade.CodigoIbge, 1, _mdfe.Emissao.Protocolo);

                if (resposta.InfEvento.CStat == 135)
                    SalvarMDFe(_mdfe, resposta.InfEvento.DhRegEvento, resposta.EnvioXmlString, resposta.RetornoXmlString);

                if (resposta.InfEvento.CStat == 631)
                {
                    var servicoFoda = new ServicoMDFeConsultaProtocolo();
                    var xmlObjeto = servicoFoda.MDFeConsultaProtocolo(ExtMDFe.Chave(mdfe));

                    xmlObjeto.ProcEventoMDFe.ForEach(e =>
                    {
                        if (e.EventoMDFe.InfEvento.TpEvento == MDFeTipoEvento.Encerramento)
                        {
                            SalvarMDFe(_mdfe,
                                e.EventoMDFe.InfEvento.DhEvento,
                                xmlObjeto.EnvioXmlString,
                                xmlObjeto.RetornoXmlString);
                        }
                    });

                    DialogBox.MostraInformacao("Evento já foi encerrado na sefaz");
                    OnFechar();
                    return;
                }

                ProcessaRespostaEncerrar(resposta);

                if (resposta.InfEvento.CStat == 135)
                    OnFechar();
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                EstaAutorizado = _mdfe.Emissao.CodigoAutorizacao == 100;
                BotaoConsultaProtocolo = !EstaAutorizado;
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
        }

        private void SalvarMDFe(MDFeEletronico mdfe, DateTime? horaEvento, string xmlEnvio, string xmlRetorno)
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                mdfe.Status = MDFeStatus.Encerrada;
                repositorio.Salvar(mdfe);

                repositorio.SalvarEvento(new MDFeEvento
                {
                    Evento = MDFeTipoEvento.Encerramento,
                    FeitoEm = horaEvento ?? DateTime.Now,
                    Mdfe = mdfe,
                    XmlEnvio = xmlEnvio,
                    XmlRetorno = xmlRetorno
                });

                transacao.Commit();
            }
        }

        private void ProcessaRespostaEncerrar(MDFeRetEventoMDFe resposta)
        {
            DialogBox.MostraInformacao(resposta.InfEvento.XMotivo);
        }

        private bool ValidaEncerrarMdfe()
        {
            if (_mdfe.Status == MDFeStatus.Encerrada)
            {
                DialogBox.MostraInformacao("MDF-e já esta Encerrada");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.Cancelada)
            {
                DialogBox.MostraInformacao("MDF-e já esta Cancelada");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.EmDigitacao)
            {
                DialogBox.MostraInformacao("MDF-e Em Digitação");
                return true;
            }

            if (_mdfe.Status == MDFeStatus.ConsultaProcessamento)
            {
                DialogBox.MostraInformacao("MDF-e Não está autorizada");
                return true;
            }
            return false;
        }

        private async void ConsultaProcessamentoAction(object obj)
        {
            await Task.Run(() => ConsultaProtocoloWebService());
        }

        private void DownloadXmlAction(object obj)
        {
            var dialog = new SaveFileDialog {Filter = @"Arquivo Xml (.xml)|*.xml"};
            dialog.ShowDialog();

            if (dialog.FileName.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não foi selecionado um caminho");
                return;
            }

            var xmlAutorizado = _mdfe.Emissao?.XmlAutorizado;

            if (xmlAutorizado.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não existe Xml Autorizado");
                return;
            }

            FuncoesXml.SalvarStringXmlParaArquivoXml(xmlAutorizado, dialog.FileName);

            DialogBox.MostraInformacao("Salvou Xml Autorizado com sucesso");
            OnFechar();
        }

        private void ConsultaProtocoloWebService()
        {
            if (_mdfe.Status == MDFeStatus.Autorizado)
            {
                DialogBox.MostraInformacao("MDF-e já esta autorizada");
                return;
            }

            try
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);


                FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(_mdfe.EmissorFiscal.EmissorFiscalMdfe);

                var servicoRecibo = new ServicoMDFeRetRecepcao();
                var retorno = servicoRecibo.MDFeRetRecepcao(_mdfe.Emissao.NumeroRecibo);

                var resposta = TrataRecibo(_mdfe.Emissao, retorno);

                SalvaEmissao(_mdfe.Emissao);

                ProcessaResposta(resposta);
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                EstaAutorizado = _mdfe.Emissao.CodigoAutorizacao == 100;
                BotaoConsultaProtocolo = !EstaAutorizado;
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
        }

        private void SalvaEmissao(MDFeEmissao emissao)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioMdfe(sessao);

                if (emissao.Autorizado)
                {
                    _mdfe.Status = MDFeStatus.Autorizado;
                    repositorio.Salvar(_mdfe);
                }
                repositorio.SalvarEmissao(emissao);

                transacao.Commit();
            }
        }

        private MDFeInfProtMDFe TrataRecibo(MDFeEmissao emissao, MDFeRetConsReciMDFe retorno)
        {
            var informacoes = retorno.ProtMdFe.InfProt;

            emissao.CodigoAutorizacao = informacoes.CStat;
            emissao.RecebidoEm = informacoes.DhRecbto;
            emissao.Motivo = informacoes.XMotivo ?? string.Empty;

            if (informacoes.CStat != 100) return informacoes;

            emissao.Autorizado = true;
            emissao.VersaoAplicativoAutorizacao = informacoes.VerAplic ?? string.Empty;
            emissao.DigestValue = informacoes.DigVal ?? string.Empty;
            emissao.Protocolo = informacoes.NProt ?? string.Empty;

            IgnoraException(() => emissao.AutonrizaXml(retorno.ProtMdFe));

            return informacoes;
        }

        private void ProcessaResposta(MDFeInfProtMDFe resposta)
        {
            if (resposta.CStat == 100)
            {
                DialogBox.MostraInformacao(resposta.XMotivo);
                return;
            }

            DialogBox.MostraInformacao(resposta.XMotivo);
        }

        private static void IgnoraException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                //igonre
            }
        }

        public void BuscarMdfe(int mdfeId)
        {

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioMdfe(sessao);

                _mdfe = repositorio.GetPeloId(mdfeId);
            }
        }

        private void AcaoEnviarEmail(object obj)
        {
            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = "MANIFESTO ELETRONICO DE DOCUMENTOS FISCAIS";
            behavior.CorpoMensagem = "Segue em anexo o DAMDFE e o XML";

            new EnvioEmailView(behavior).ShowDialog();

            OnFechar();
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<Email> e)
        {
            if (!(sender is EnvioEmailBehavior behavior))
            {
                return;
            }

            new MDFeImpressaoHelper().EnviaEmail(_mdfe, e, behavior.Assunto, behavior.CorpoMensagem);
        }
    }
}