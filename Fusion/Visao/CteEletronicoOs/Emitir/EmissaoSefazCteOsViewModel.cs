using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DFe.CertificadosDigitais.Implementacao;
using DFe.DocumentosEletronicos.CTe.Classes.Extensoes;
using DFe.DocumentosEletronicos.CTe.Classes.Retorno.Consulta;
using DFe.DocumentosEletronicos.CTe.CTeOS;
using DFe.DocumentosEletronicos.CTe.CTeOS.Extensoes;
using DFe.DocumentosEletronicos.CTe.CTeOS.Retorno;
using DFe.DocumentosEletronicos.CTe.CTeOS.Servicos.Autorizacao;
using DFe.DocumentosEletronicos.CTe.Facade;
using DFe.DocumentosEletronicos.CTe.Servicos.ConsultaProtocoloCTe;
using DFe.DocumentosEletronicos.CTe.Servicos.EnviarCTe;
using DFe.DocumentosEletronicos.Entidades;
using DFe.DocumentosEletronicos.Flags;
using DFe.Utils;
using DFe.Utils.Assinatura;
using FusionCore.FusionAdm.ConfiguracoesDfe;
using FusionCore.FusionAdm.CteEletronicoOs.Configuracao;
using FusionCore.FusionAdm.CteEletronicoOs.Configuracao.Factory;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Extencoes;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Historicos;
using FusionCore.FusionAdm.CteEletronicoOs.NumeroFiscal;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Shared.NFe.Utils.InfRespTec;

namespace Fusion.Visao.CteEletronicoOs.Emitir
{
    public class EmissaoSefazCteOsViewModel : ViewModel
    {
        private readonly CteOs _cteOs;

        private string _chaveEmAutorizacao;
        private bool _envioOk;
        private bool _retornoOk;
        private bool _emProcessamento;
        private string _textoInformativo;
        private CteOsEmissaoHistorico _emissaoHistorico;
        private DFeCertificadoDigital _certificado;
        private CTeOsConfig _config;

        public EmissaoSefazCteOsViewModel(CteOs cteOs)
        {
            _cteOs = cteOs;
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

        public void ContentRendered()
        {
            TextoInformativo = "Nenhuma informação disponível";

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCteOs(sessao);
                _emissaoHistorico = repositorio.BuscaUltimaEmissao(_cteOs);
            }

            if (_emissaoHistorico != null && _emissaoHistorico.Finalizada == false)
            {
                EmissaoEmAndamento = true;
                EnvioOk = _emissaoHistorico.EnviadoEm != null;
                TextoInformativo = "Existe uma emissão pendente. Clique em no botão para para conclui-la";
                ChaveEmAutorizacao = _emissaoHistorico.Chave;
            }

            EmProcessamento = false;
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
                    AlocarNumeracaoFiscal();

                    var emissor = _cteOs.Perfil.EmissorFiscal;
                    _config = FactoryCTeConfig.CriarCteConfig(emissor, _cteOs.TipoEmissao);
                    _certificado = FactoryCTeConfig.CriarCertificadoDigital(emissor);

                    var facade = new CTeFacade(_config, _certificado);

                    facade.AntesDeEnviarCteOsHandler += AntesEnviarDocumentoHandler;
                    facade.AntesDeValidarSchemaCteOsHandler += AntesValidarSchemaHandler;
                    facade.AntesDeConculstarHandler += AntesDeConsultarHandler;

                    var retornoConsultaCTe = VerificaHistorico(_emissaoHistorico, facade);

                    if (retornoConsultaCTe.IsAutorizado() || retornoConsultaCTe.IsDenegada())
                    {
                        FinalizaEmissaoEHistorico(retornoConsultaCTe);
                        TextoInformativo = retornoConsultaCTe.xMotivo;
                        RetornoOk = true;
                        return;
                    }

                    if (retornoConsultaCTe.IsRejeicao999())
                    {
                        TextoInformativo = "Tente Novamente Rejeição: 999";
                        RetornoOk = true;
                        return;
                    }

                    if (retornoConsultaCTe.IsRejeicao())
                    {
                        FinalizaHistorico(retornoConsultaCTe.ObterXmlString());
                        TextoInformativo = retornoConsultaCTe.xMotivo;
                        RetornoOk = true;
                        return;
                    }

                    AutorizarDocumento(facade);
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

        private void AntesValidarSchemaHandler(object sender, AntesDeValidarSchema e)
        {
            var cte = e.Cte;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var uf = new RepositorioEstado(sessao).GetPelaSigla(cte.InfCte.emit.enderEmit.UF.GetSiglaUfString());
                var tipoAmbiente = cte.InfCte.ide.tpAmb == TipoAmbiente.Homologacao
                    ? FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Homologacao
                    : FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Producao;

                if (new RepositorioConfiguracaoDfe(sessao).AdicionarQrCode(uf, tipoAmbiente, TipoDocumentoFiscalEletronico.CTeOs))
                {
                    cte.infCTeSupl = cte.QrCode(_certificado.ObterCertificadoDigital(), _config);
                }  
            }
            
        }

        private void AutorizarDocumento(CTeFacade facade)
        {
            var cteZeus = _cteOs.ToDFe();

            
            facade.ChaveAntesDeAssinarEventHandler += delegate(object sender, ChaveAntesDeAssinarEventHandler handler)
            {
                GerarHashCsrt(_cteOs, cteZeus, handler.Chave);
            };
            var retorno = facade.Enviar(cteZeus);

            FinalizaEmissaoEHistorico(retorno);
            RetornoOk = true;

            if (retorno.IsRejeicao999())
            {
                TextoInformativo = "Tente Novamente Rejeição: 999";
                RetornoOk = true;
                return;
            }

            if (retorno.IsRejeicao())
            {
                TextoInformativo = retorno?.protCTe?.infProt?.xMotivo ?? retorno?.xMotivo;
                return;
            }

            if (retorno.IsAutorizado() || retorno.IsDenegada())
            {
                IsAutorizado = true;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    new CteOsEletronicaOpcoes(new CteOsEletronicaOpcoesModel(_cteOs)).ShowDialog();
                    OnFechar();
                });
            }
        }

        private void GerarHashCsrt(CteOs cteOs, CTeOS cteZeus, string chave)
        {
            if (cteZeus.InfCte.infRespTec == null) return;

            var ufId = cteOs.Emitente.EstadoDTO.Id;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioResponsavelTecnico = new RepositorioResponsavelTecnico(sessao);

                if (repositorioResponsavelTecnico.ExisteCsrt(ufId, TipoDocumentoFiscalEletronico.CTeOs) == false) return;

                var reponsavelTecnico =
                    repositorioResponsavelTecnico.BuscarPorUf(ufId);

                cteZeus.InfCte.infRespTec.hashCSRT = GerarHashCSRT.HashCSRT(reponsavelTecnico.Csrt, chave);
                cteZeus.InfCte.infRespTec.idCSRT = reponsavelTecnico.CsrtId.ToString("D2");
            }
        }

        public bool IsAutorizado { get; set; }

        private void AlocarNumeracaoFiscal()
        {
            if (_cteOs.NumeroEmissao == 0)
                new AlocarNumeroFiscalCteOs().Alocar(_cteOs);
        }

        private void FinalizaEmissaoEHistorico(retConsSitCTe retorno)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                _emissaoHistorico.Finalizada = true;
                _emissaoHistorico.XmlRetorno = retorno.ObterXmlString();

                repositorio.Salvar(_emissaoHistorico);

                if (retorno.IsAutorizado() || retorno.IsDenegada())
                {
                    _cteOs.Status = Status.Autorizada;

                    if (retorno.IsDenegada())
                        _cteOs.Status = Status.Denegada;

                    var emissaoFinalizada = CriaEmissao(retorno);

                    repositorio.Salvar(emissaoFinalizada);
                    repositorio.Salvar(_cteOs);
                }

                transacao.Commit();
            }
        }


        private void FinalizaEmissaoEHistorico(retCTeOS retorno)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                _emissaoHistorico.Finalizada = true;
                _emissaoHistorico.XmlRetorno = retorno.ObterXmlString();

                repositorio.Salvar(_emissaoHistorico);

                if (retorno.IsAutorizado() || retorno.IsDenegada())
                {
                    var emissaoFinalizada = CriaEmissao(retorno);
                    repositorio.Salvar(emissaoFinalizada);

                    _cteOs.Status = Status.Autorizada;
                    if (retorno.IsDenegada())
                        _cteOs.Status = Status.Denegada;

                    repositorio.Salvar(_cteOs);
                }

                transacao.Commit();
            }
        }

        private CteOsEmissaoFinalizada CriaEmissao(retCTeOS retorno)
        {
            var finalizaEmissao = new CteOsEmissaoFinalizada
            {
                AmbienteSefaz = retorno.tpAmb == TipoAmbiente.Homologacao
                    ? FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Homologacao
                    : FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Producao,

                CteOs = _cteOs,
                TipoEmissao = _emissaoHistorico.TipoEmissao,
                Chave = _emissaoHistorico.Chave,
                Autorizado = true,
                XmlAutorizado = MontarXml(retorno),
                CriadoEm = DateTime.Now,
                EnviadoEm = retorno?.protCTe?.infProt?.dhRecbto ?? DateTime.Now,
                Protocolo = retorno?.protCTe?.infProt?.nProt ?? string.Empty
            };

            _cteOs.Emissao = finalizaEmissao;

            return finalizaEmissao;
        }

        private CteOsEmissaoFinalizada CriaEmissao(retConsSitCTe retorno)
        {
            var finalizaEmissao = new CteOsEmissaoFinalizada
            {
                AmbienteSefaz = retorno.tpAmb == TipoAmbiente.Homologacao
                    ? FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Homologacao
                    : FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente.Producao,

                CteOs = _cteOs,
                TipoEmissao = _emissaoHistorico.TipoEmissao,
                Chave = _emissaoHistorico.Chave,
                Autorizado = true,
                XmlAutorizado = MontarXml(retorno),
                CriadoEm = DateTime.Now,
                EnviadoEm = retorno?.protCTe?.infProt?.dhRecbto ?? DateTime.Now,
                Protocolo = retorno?.protCTe?.infProt?.nProt ?? string.Empty
            };

            _cteOs.Emissao = finalizaEmissao;

            return finalizaEmissao;
        }

        private string MontarXml(retConsSitCTe retorno)
        {
            var cteProc = new cteOSProc
            {
                CTeOS = FuncoesXml.XmlStringParaClasse<CTeOS>(_emissaoHistorico.XmlEnvio),
                versao = VersaoServico.Versao300,
                protCTe = retorno.protCTe
            };

            return cteProc.ObterXmlString();
        }

        private string MontarXml(retCTeOS retorno)
        {
            var cteProc = new cteOSProc
            {
                CTeOS = FuncoesXml.XmlStringParaClasse<CTeOS>(_emissaoHistorico.XmlEnvio),
                versao = VersaoServico.Versao300,
                protCTe = retorno.protCTe
            };

            return cteProc.ObterXmlString();
        }
        
        private void AntesDeConsultarHandler(object sender, AntesDeConsultar e)
        {
            TextoInformativo = "Preparando para consultar documento";

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _emissaoHistorico.EnviadoEm = DateTime.Now;
                new RepositorioCteOs(sessao).Salvar(_emissaoHistorico);

                transacao.Commit();
            }
        }

        private void FinalizaHistorico(string retorno)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                _emissaoHistorico.Finalizada = true;
                _emissaoHistorico.XmlRetorno = retorno;

                repositorio.Salvar(_emissaoHistorico);

                transacao.Commit();
            }
        }

        private retConsSitCTe VerificaHistorico(CteOsEmissaoHistorico emissaoHistorico, CTeFacade facade)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCteOs(sessao);
                _emissaoHistorico = repositorio.BuscaUltimaEmissao(_cteOs);
                emissaoHistorico = _emissaoHistorico;
            }

            if (emissaoHistorico == null || emissaoHistorico.Finalizada)
            {
                return null;
            }

            EnvioOk = true;
            TextoInformativo = "Estou consultando a chave na SEFAZ";

            return facade.Consulta(emissaoHistorico.Chave);
        }

        private void AntesEnviarDocumentoHandler(object sender, AntesDeEnviarCteOs e)
        {
            TextoInformativo = "Preparando para enviar documento";

            var emissorFiscal = _cteOs.Perfil.EmissorFiscal;
            var emissorFiscalCteOs = emissorFiscal.EmissorFiscalCteOs;

            ChaveEmAutorizacao = e.CteOsOs.Chave();

            var emissor = _cteOs.Perfil.EmissorFiscal;
            var config = FactoryCTeConfig.CriarCteConfig(emissor, _cteOs.TipoEmissao);

            var emissaoHistorico = new CteOsEmissaoHistorico
            {
                CteOs = _cteOs,
                AmbienteSefaz = emissorFiscalCteOs.Ambiente,
                CriadoEm = DateTime.Now,
                TipoEmissao = _cteOs.TipoEmissao,
                Chave = ChaveEmAutorizacao,
                EnviadoEm = DateTime.Now,
                XmlEnvio = e.CteOsOs.ObterXmlString(config),
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                repositorio.Salvar(emissaoHistorico);
                transacao.Commit();
            }

            _emissaoHistorico = emissaoHistorico;
            EnvioOk = true;
        }
    }
}