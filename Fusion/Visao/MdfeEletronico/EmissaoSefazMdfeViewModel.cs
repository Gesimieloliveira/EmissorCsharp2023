using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DFe.Classes.Extensoes;
using DFe.Classes.Flags;
using DFe.Utils;
using DFe.Utils.Assinatura;
using FusionCore.FusionAdm.ConfiguracoesDfe;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.Factory;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using MDFe.Classes.Extencoes;
using MDFe.Classes.Retorno.MDFeConsultaProtocolo;
using MDFe.Classes.Retorno.MDFeRetRecepcao;
using MDFe.Classes.Servicos.Autorizacao;
using MDFe.Servicos.ConsultaProtocoloMDFe;
using MDFe.Servicos.RecepcaoMDFe;
using MDFe.Servicos.RetRecepcaoMDFe;
using MDFe.Utils.Configuracoes;
using NHibernate.Util;
using Shared.NFe.Utils.InfRespTec;

namespace Fusion.Visao.MdfeEletronico
{
    public class EmissaoSefazMdfeViewModel : ViewModel
    {
        private MDFeEletronico _mdfe;
        private bool _emProcessamento;
        private string _chaveEmAutorizacao;
        private bool _envioOk;
        private bool _retornoOk;
        private string _textoInformativo;
        private MDFeEmissaoHistorico _emissaoHistorio;

        public event EventHandler<EmissaoSefazMdfeViewModel> EmissaoAutorizadaHandler;
        public event EventHandler<MDFeEletronico> EventoAlocouNumeracao;

        public EmissaoSefazMdfeViewModel(EventHandler<MDFeEletronico> eventoAlocouNumeracao, EventHandler<EmissaoSefazMdfeViewModel> emissaoAutorizadaHandler)
        {
            EmissaoAutorizadaHandler = emissaoAutorizadaHandler;
            EventoAlocouNumeracao = eventoAlocouNumeracao;
        }

        public void AtualizarInformacoesMDFe(MDFeEletronico mdfe)
        {
            using (var repositorio = new RepositorioMdfe(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                repositorio.Refresh(mdfe);
            }

            _mdfe = mdfe;
        }

        public string ChaveEmAutorizacao
        {
            get => !string.IsNullOrWhiteSpace(_chaveEmAutorizacao) ? _chaveEmAutorizacao : "Ainda não gerei!";
            set
            {
                _chaveEmAutorizacao = value;
                PropriedadeAlterada();
            }
        }

        public bool EnvioOk
        {
            get => _envioOk;
            set
            {
                _envioOk = value;
                PropriedadeAlterada();
            }
        }

        public bool RetornoOk
        {
            get => _retornoOk;
            set
            {
                _retornoOk = value;
                PropriedadeAlterada();
            }
        }

        public bool EmProcessamento
        {
            get => _emProcessamento;
            set
            {
                _emProcessamento = value;
                PropriedadeAlterada();
            }
        }

        public string TextoInformativo
        {
            get => _textoInformativo;
            set
            {
                _textoInformativo = value;
                PropriedadeAlterada();
            }
        }

        private bool EmissaoEmAndamento { get; set; }

        public ICommand CommandEmitir => GetSimpleCommand(EmitirAction);
        public bool IsAutorizado { get; set; }

        public void ContentRendered()
        {
            TextoInformativo = "Nenhuma informação disponível";

            BuscaUltimoHistoricoAberto();

            if (_emissaoHistorio != null && _emissaoHistorio.Finalizada == false)
            {
                EmissaoEmAndamento = true;
                EnvioOk = _emissaoHistorio.EnviadoEm != null;
                TextoInformativo = "Existe uma emissão pendente. Clique em no botão para para conclui-la";
                ChaveEmAutorizacao = _emissaoHistorio.Chave;
            }

            EmProcessamento = false;
        }

        private void BuscaUltimoHistoricoAberto()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);
                _emissaoHistorio = repositorioMdfe.BuscaUltimaEmissaoHistorico(_mdfe);
            }
        }

        private void EmitirAction(object obj)
        {
            TextoInformativo = "Estou iniciando a emissão";

            EmProcessamento = true;
            EmissaoEmAndamento = true;
            EnvioOk = false;
            RetornoOk = false;

            Task.Run(() =>
            {
                try
                {
                    CarregarConfiguracoesMDFe();

                    BuscaUltimoHistoricoAberto();
                    var retornoConsultaMDFe = VerificarHistorico(_emissaoHistorio);

                    if (retornoConsultaMDFe.IsAutorizado())
                    {
                        IsAutorizado = true;
                        FinalizaEmissaoEHistorico(retornoConsultaMDFe);
                        TextoInformativo = retornoConsultaMDFe.XMotivo;
                        RetornoOk = true;
                        OnEmissaoAutorizadaHandler();
                        return;
                    }

                    if (retornoConsultaMDFe.IsRejeicao999())
                    {
                        TextoInformativo = "Tente Novamente Rejeição: 999";
                        RetornoOk = true;
                        return;
                    }

                    if (retornoConsultaMDFe.IsTemNfeCancelada())
                    {
                        FinalizaHistorico(retornoConsultaMDFe.RetornoXmlString, retornoConsultaMDFe.XMotivo.ApenasNumerosString());
                        TextoInformativo = $"Foi detectado uma NF-e cancelada, buscar nf-e pela chave\n {retornoConsultaMDFe.XMotivo}";
                        RetornoOk = true;
                        return;
                    }

                    if (retornoConsultaMDFe.IsRejeicao())
                    {
                        FinalizaHistorico(retornoConsultaMDFe.RetornoXmlString);
                        TextoInformativo = retornoConsultaMDFe.XMotivo;
                        RetornoOk = true;
                        return;
                    }

                    AutorizarDocumento();
                }
                catch (WebException e)
                {
                    CertificadoDigital.ClearCache();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TextoInformativo = e.Message;
                        DialogBox.MostraErro("Falha de comunicação com a SEFAZ", e);
                    });
                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TextoInformativo = e.Message;
                        DialogBox.MostraErro("Falha ao autorizar: " + e.Message, e);
                    });
                }
                finally
                {
                    EmProcessamento = false;
                }
            });
        }

        private void AutorizarDocumento()
        {
            AlocarNumeracaoFiscal();
            CarregarConfiguracoesMDFe();

            var servicoRecepcao = new ServicoMDFeRecepcao();
            var mdfeZeus = _mdfe.ToZeusMdfe();

            servicoRecepcao.GerouChave += delegate (object sender, string chave)
            {
                ChaveEmAutorizacao = chave;
                GerarHashCsrt(_mdfe, mdfeZeus, chave);
            };


            servicoRecepcao.AntesDeEnviar += delegate (object sender, AntesDeEnviar enviar)
            {
                TextoInformativo = "Emissão ok. Vou autorizar a NF-e agora";
                var mdfe = enviar.enviMdFe.MDFe;

                GeraQrCode(mdfe);

                var emitirMdfe = new EmitirHistoricoMdfe();

                _emissaoHistorio = emitirMdfe.GerarHistorico(_mdfe, mdfe.Chave(), enviar.enviMdFe.XmlString());
            };

            var retornoEnvioLote = servicoRecepcao.MDFeRecepcao(_mdfe.Id, mdfeZeus);
            EnvioOk = true;

            Thread.Sleep(5000);

            if (retornoEnvioLote == null)
            {
                throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");
            }

            _emissaoHistorio.NumeroRecibo = retornoEnvioLote.InfRec?.NRec ?? string.Empty;
            _emissaoHistorio.XmlLote = retornoEnvioLote.RetornoXmlString;

            SalvarHistorico();

            if (retornoEnvioLote.CStat != 103)
            {
                TextoInformativo = retornoEnvioLote.XMotivo;
                RetornoOk = true;
                return;
            }

            MDFeRetConsReciMDFe retornoConsultaRecepcao = null;

            retornoConsultaRecepcao = ConsultaRecibo(retornoConsultaRecepcao);

            if (retornoConsultaRecepcao.ProtMdFe.InfProt.CStat != 100)
            {
                TextoInformativo = retornoConsultaRecepcao.ProtMdFe.InfProt.XMotivo;
                RetornoOk = true;

                if (retornoConsultaRecepcao.ProtMdFe.InfProt.CStat == 677)
                {
                    FinalizaHistorico(retornoConsultaRecepcao.RetornoXmlString, retornoConsultaRecepcao.ProtMdFe.InfProt.XMotivo.ApenasNumerosString());
                    TextoInformativo = $"Foi detectado uma NF-e cancelada, buscar nf-e pela chave\n {retornoConsultaRecepcao.ProtMdFe.InfProt.XMotivo}";
                    return;
                }

                FinalizaHistorico(retornoConsultaRecepcao.RetornoXmlString);
                return;
            }

            FinalizaEmissaoEHistorico(retornoConsultaRecepcao);
            TextoInformativo = retornoConsultaRecepcao.ProtMdFe.InfProt.XMotivo;
            RetornoOk = true;
            OnEmissaoAutorizadaHandler();
        }

        private MDFeRetConsReciMDFe ConsultaRecibo(MDFeRetConsReciMDFe retornoConsultaRecepcao)
        {
            if (!_emissaoHistorio.NumeroRecibo.IsNullOrEmpty())
            {
                var servicoRecibo = new ServicoMDFeRetRecepcao();
                retornoConsultaRecepcao = servicoRecibo.MDFeRetRecepcao(_emissaoHistorio.NumeroRecibo);

                if (LoteEmProcessamento(retornoConsultaRecepcao))
                {
                    TextoInformativo = "Servidor da SEFAZ está sonolento, espere mais um pouco";
                    Thread.Sleep(7000);
                    retornoConsultaRecepcao = servicoRecibo.MDFeRetRecepcao(_emissaoHistorio.NumeroRecibo);
                }

                if (LoteEmProcessamento(retornoConsultaRecepcao))
                {
                    TextoInformativo = "Servidor da SEFAZ está sonolento, espere mais um pouco";
                    Thread.Sleep(15000);
                    retornoConsultaRecepcao = servicoRecibo.MDFeRetRecepcao(_emissaoHistorio.NumeroRecibo);
                }
            }

            return retornoConsultaRecepcao;
        }

        private bool LoteEmProcessamento(MDFeRetConsReciMDFe retornoConsultaRecepcao)
        {
            return retornoConsultaRecepcao.CStat == 105;
        }

        private void SalvarHistorico()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);
                repositorioMdfe.SalvarHistorico(_emissaoHistorio);

                transacao.Commit();
            }
        }

        private void AlocarNumeracaoFiscal()
        {
            if (_mdfe.NumeroFiscalEmissao == 0)
            {
                new AlocarNumeroFiscalMDFe().Alocar(_mdfe);
                OnEventoAlocouNumeracao(_mdfe);
            }
        }

        private static void GeraQrCode(global::MDFe.Classes.Informacoes.MDFe mdfe)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var uf = new RepositorioEstado(sessao).GetPelaSigla(mdfe.InfMDFe.Emit.EnderEmit.UF.GetSiglaUfString());
                var tipoAmbiente = mdfe.InfMDFe.Ide.TpAmb == TipoAmbiente.Homologacao
                    ? FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Homologacao
                    : FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Producao;

                if (new RepositorioConfiguracaoDfe(sessao).AdicionarQrCode(uf, tipoAmbiente, TipoDocumentoFiscalEletronico.MDFe)
                )
                {
                    mdfe.infMDFeSupl = mdfe.QrCode(MDFeConfiguracao.X509Certificate2);
                }
            }
        }

        private void GerarHashCsrt(MDFeEletronico mdfe, global::MDFe.Classes.Informacoes.MDFe mdfeZeus, string chave)
        {
            if (mdfeZeus.InfMDFe.infRespTec == null) return;

            var ufId = mdfe.Emitente.Empresa.EstadoDTO.Id;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioResponsavelTecnico = new RepositorioResponsavelTecnico(sessao);

                if (repositorioResponsavelTecnico.ExisteCsrt(ufId, TipoDocumentoFiscalEletronico.MDFe) == false) return;

                var reponsavelTecnico =
                    repositorioResponsavelTecnico.BuscarPorUf(ufId);

                mdfeZeus.InfMDFe.infRespTec.hashCSRT = GerarHashCSRT.HashCSRT(reponsavelTecnico.Csrt, chave);
                mdfeZeus.InfMDFe.infRespTec.idCSRT = reponsavelTecnico.CsrtId;
            }
        }

        private void CarregarConfiguracoesMDFe()
        {
            var emissor = _mdfe.EmissorFiscal;
            FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(emissor.EmissorFiscalMdfe);
        }

        private void FinalizaHistorico(string xmlRetorno, string chave = null)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);
                _emissaoHistorio.Finalizado(xmlRetorno);

                repositorioMdfe.SalvarHistorico(_emissaoHistorio);

                if (chave != null)
                {
                    var descarregamento = _mdfe.Descarregamentos.Where(x => x.ChaveDocumento == chave).FirstOrNull() as MDFeDescarregamento;

                    if (descarregamento != null)
                    {
                        descarregamento.DocumentoCanceladoPelaSefaz();
                        repositorioMdfe.AtualizarDescarregamento(descarregamento);
                    }
                }

                transacao.Commit();
            }
        }

        private void FinalizaEmissaoEHistorico(MDFeRetConsSitMDFe retornoConsultaMDFe)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                _emissaoHistorio.XmlRetorno = retornoConsultaMDFe.RetornoXmlString;
                _emissaoHistorio.Finalizada = true;
                repositorioMdfe.SalvarHistorico(_emissaoHistorio);

                if (retornoConsultaMDFe.IsAutorizado())
                {
                    _mdfe.Status = MDFeStatus.Autorizado;

                    var emissaoFinalizada = CriaEmissao(retornoConsultaMDFe);
                    
                    repositorioMdfe.SalvarEmissao(emissaoFinalizada);
                    repositorioMdfe.Salvar(_mdfe);
                }

                transacao.Commit();
            }
        }

        private void FinalizaEmissaoEHistorico(MDFeRetConsReciMDFe retornoConsultaMDFe)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                _emissaoHistorio.Finalizada = true;
                _emissaoHistorio.XmlRetorno = retornoConsultaMDFe.RetornoXmlString;

                repositorioMdfe.SalvarHistorico(_emissaoHistorio);

                _mdfe.Status = MDFeStatus.Autorizado;

                var emissaoFinalizada = CriaEmissao(retornoConsultaMDFe);
                repositorioMdfe.SalvarEmissao(emissaoFinalizada);
                repositorioMdfe.Salvar(_mdfe);

                transacao.Commit();
            }
        }

        private MDFeEmissao CriaEmissao(MDFeRetConsReciMDFe retornoConsultaMDFe)
        {
            var chaveSefaz = new ChaveSefaz(_emissaoHistorio.Chave);

            var finalizaEmissao = new MDFeEmissao
            {
                MDFeEletronico = _mdfe,
                Chave = _emissaoHistorio.Chave,
                Ambiente = _emissaoHistorio.AmbienteSefaz,
                Autorizado = true,
                CodigoAutorizacao = retornoConsultaMDFe.ProtMdFe.InfProt.CStat,
                CodigoNumerico = chaveSefaz.GetCodigoNumerico(),
                DigestValue = retornoConsultaMDFe.ProtMdFe.InfProt.DigVal,
                DigitoVerificador = (byte)chaveSefaz.Dv,
                EmitidoEm = _mdfe.EmissaoEm,
                ModeloManifesto = MDFeModeloManifesto.MDFe,
                Motivo = retornoConsultaMDFe.XMotivo,
                NumeroDocumento = Convert.ToInt32(chaveSefaz.GetNumeroFiscal()),
                NumeroRecibo = _emissaoHistorio.NumeroRecibo,
                Protocolo = retornoConsultaMDFe.ProtMdFe.InfProt.NProt,
                RecebidoEm = retornoConsultaMDFe.ProtMdFe.InfProt.DhRecbto,
                Serie = chaveSefaz.GetSerie(),
                TagId = $"MDFe{chaveSefaz.Chave}",
                TipoEmissao = MDFeTipoEmissao.Normal,
                VersaoAplicativoAutorizacao = retornoConsultaMDFe.ProtMdFe.InfProt.VerAplic,
                VersaoLayout = retornoConsultaMDFe.ProtMdFe.Versao,
                XmlAssinado = FuncoesXml.XmlStringParaClasse<MDFeEnviMDFe>(_emissaoHistorio.XmlEnvio).MDFe.XmlString(),
                XmlAutorizado = string.Empty
            };

            finalizaEmissao.AutonrizaXml(retornoConsultaMDFe.ProtMdFe);

            return finalizaEmissao;
        }

        private MDFeEmissao CriaEmissao(MDFeRetConsSitMDFe retornoConsultaMDFe)
        {
            var chaveSefaz = new ChaveSefaz(_emissaoHistorio.Chave);

            var finalizaEmissao = new MDFeEmissao
            {
                MDFeEletronico = _mdfe,
                Chave = _emissaoHistorio.Chave,
                Ambiente = _emissaoHistorio.AmbienteSefaz,
                Autorizado = true,
                CodigoAutorizacao = retornoConsultaMDFe.ProtMDFe.InfProt.CStat,
                CodigoNumerico = chaveSefaz.GetCodigoNumerico(),
                DigestValue = retornoConsultaMDFe.ProtMDFe.InfProt.DigVal, 
                DigitoVerificador = (byte) chaveSefaz.Dv,
                EmitidoEm = _mdfe.EmissaoEm,
                ModeloManifesto = MDFeModeloManifesto.MDFe,
                Motivo = retornoConsultaMDFe.XMotivo,
                NumeroDocumento = Convert.ToInt32(chaveSefaz.GetNumeroFiscal()),
                NumeroRecibo = _emissaoHistorio.NumeroRecibo,
                Protocolo = retornoConsultaMDFe.ProtMDFe.InfProt.NProt,
                RecebidoEm = retornoConsultaMDFe.ProtMDFe.InfProt.DhRecbto,
                Serie = chaveSefaz.GetSerie(),
                TagId = $"MDFe{chaveSefaz.Chave}",
                TipoEmissao = MDFeTipoEmissao.Normal,
                VersaoAplicativoAutorizacao = retornoConsultaMDFe.ProtMDFe.InfProt.VerAplic,
                VersaoLayout = retornoConsultaMDFe.ProtMDFe.Versao,
                XmlAssinado = FuncoesXml.XmlStringParaClasse<MDFeEnviMDFe>(_emissaoHistorio.XmlEnvio).MDFe.XmlString(),
                XmlAutorizado = string.Empty
            };

            finalizaEmissao.AutonrizaXml(retornoConsultaMDFe.ProtMDFe);

            return finalizaEmissao;
        }

        private MDFeRetConsSitMDFe VerificarHistorico(MDFeEmissaoHistorico emissaoHistorio)
        {
            if (emissaoHistorio == null || emissaoHistorio.Finalizada)
            {
                return null;
            }

            EnvioOk = true;
            TextoInformativo = "Estou consultando a chave na SEFAZ";

            var servicoConsultaProtocolo = new ServicoMDFeConsultaProtocolo();
            var retorno = servicoConsultaProtocolo.MDFeConsultaProtocolo(emissaoHistorio.Chave);

            return retorno;
        }

        protected virtual void OnEmissaoAutorizadaHandler()
        {
            EmissaoAutorizadaHandler?.Invoke(this, this);
        }

        protected virtual void OnEventoAlocouNumeracao(MDFeEletronico mdfe)
        {
            EventoAlocouNumeracao?.Invoke(this, mdfe);
        }

        public int ObterIdMdfe()
        {
            return _mdfe.Id;
        }
    }
}