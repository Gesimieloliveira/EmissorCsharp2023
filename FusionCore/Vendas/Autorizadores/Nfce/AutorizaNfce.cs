using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml;
using DFe.DocumentosEletronicos.CTe.Validacao;
using DFe.Utils;
using DFe.Utils.Assinatura;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.CteEletronicoOs.Configuracao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Aplicacao;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Infraestrutura;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionCore.Vendas.Servicos;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Autorizacao;
using NFe.Wsdl;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class AutorizaNfce
    {
        private readonly FaturamentoVenda _venda;
        private EmissorFiscal _emissorFiscal;

        public AutorizaNfce(FaturamentoVenda venda)
        {
            _venda = venda;
        }

        public void AutorizarNaSefaz()
        {
            try
            {
                var cfg = CarregarConfiguracaoServicoZeus();

                var cupomFiscalHistorico = CarregarCupomFiscalHistorico();

                if (ContingenciaAtiva.EstaAtiva())
                {
                    AutorizaEmContingencia(cupomFiscalHistorico);
                    return;
                }

                var tipoSincronizacao = TipoSincronizacao.Sincrono;

                if (tipoSincronizacao == TipoSincronizacao.Assincrono)
                {
                    EnviarLote(cupomFiscalHistorico, cfg.GetConfiguracao());

                    ConsultaLoteSefaz(cfg, cupomFiscalHistorico);
                }

                if (tipoSincronizacao == TipoSincronizacao.Sincrono)
                {
                    EnviarSincrono(cupomFiscalHistorico, cfg.GetConfiguracao());
                }
                

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    try
                    {
                        repositorioCupomFiscal.SalvarOuAlterarHistorico(cupomFiscalHistorico);
                        var cupomFiscal = cupomFiscalHistorico.CupomFiscal;
                        

                        if (cupomFiscalHistorico.Autorizado())
                        {
                            cupomFiscal.Autorizada();
                            FinalizaCupomFiscal(cupomFiscal, cupomFiscalHistorico, repositorioCupomFiscal, new RepositorioFaturamento(sessao));
                        }

                        if (cupomFiscalHistorico.Denegado())
                        {
                            cupomFiscal.Denegada();
                            FinalizaCupomFiscal(cupomFiscal, cupomFiscalHistorico, repositorioCupomFiscal, new RepositorioFaturamento(sessao));
                        }

                        transacao.Commit();
                    }
                    catch
                    {
                        transacao.Rollback();
                        sessao.Clear();

                        sessao.Load(cupomFiscalHistorico, cupomFiscalHistorico.Id);
                        sessao.Load(_venda, _venda.Id);
                        throw;
                    }
                }

                VerificaSeTemRejeicao(cupomFiscalHistorico);
            }
            catch (WebException e)
            {
                CertificadoDigital.ClearCache();
                AtivarContingencia();
                ConverterVendaAtualParaContingencia();
                throw new WebException("Houve um erro de conexão com a Sefaz, o cupom foi emitido em modo Contingência e nos\n próximos 40 minutos iremos manter a contingência ativa.", e);
            }
        }

        private void ConverterVendaAtualParaContingencia()
        {
            new AdicionarContingenciaNaVenda(_venda).Adicionar();

            ConverteNfceEmContingencia.Converter(_venda);
        }

        private void AutorizaEmContingencia(CupomFiscalHistorico cupomFiscalHistorico)
        {
            var cupomFiscal = cupomFiscalHistorico.CupomFiscal;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _venda.SituacaoFiscalAutorizadoSemInternet();
                cupomFiscal.AutorizadaSemInternet();

                new RepositorioFaturamento(sessao).Salvar(_venda);
                new RepositorioCupomFiscal(sessao).SalvarOuAlterar(cupomFiscal);
                transacao.Commit();
            }
        }

        private static void AtivarContingencia()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                ITodasContingenciasNfce todasContingencias = new TodasContingencias(sessao);
                IAtivarContingenciaDominio ativarContingenciaDominio = new AtivarContingenciaDominio();

                IAtivarContingenciaAplicacao ativarContingencia =
                    new AtivarContingenciaAplicacao(ativarContingenciaDominio, todasContingencias);

                ativarContingencia.Ativar();

                transacao.Commit();
            }
        }

        private void VerificaSeTemRejeicao(CupomFiscalHistorico cupomFiscalHistorico)
        {
            try
            {
                cupomFiscalHistorico.ThrowInvalidOperationOutrasRejeicoes();
            }
            catch
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioFaturamento = new RepositorioFaturamento(sessao);
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    var cupomFiscal = cupomFiscalHistorico.CupomFiscal;

                    cupomFiscal.ComRejeicao();

                    repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
                    repositorioFaturamento.Salvar(_venda);
                    transacao.Commit();
                }

                throw;
            }
        }

        private void FinalizaCupomFiscal(CupomFiscal cupomFiscal,
            CupomFiscalHistorico cupomFiscalHistorico,
            RepositorioCupomFiscal repositorioCupomFiscal,
            RepositorioFaturamento repositorioFaturamento)
        {
            cupomFiscal.ComCupomFinalizado(new CupomFiscalFinalizado(
                cupomFiscal,
                cupomFiscalHistorico.Chave,
                cupomFiscalHistorico.ObterProtocolo(),
                cupomFiscalHistorico.ObterDataReciboEm(),
                cupomFiscalHistorico.MontarXmlProcessado()));

            repositorioCupomFiscal.SalvarFinalizacao(cupomFiscal.CupomFiscalFinalizado);
            repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);

            ConverteSituacaoFiscalParaVenda(cupomFiscal.SituacaoFiscal);
            repositorioFaturamento.Salvar(_venda);
        }

        private void ConverteSituacaoFiscalParaVenda(SituacaoFiscal situacaoFiscal)
        {
            switch (situacaoFiscal)
            {
                case SituacaoFiscal.Aberta:
                    _venda.SituacaoFiscalNaoEnviado();
                    break;
                case SituacaoFiscal.Autorizada:
                    _venda.SituacaoFiscalAutorizado();
                    break;
                case SituacaoFiscal.AutorizadaSemInternet:
                    _venda.SituacaoFiscalAutorizadoSemInternet();
                    break;
                case SituacaoFiscal.Cancelado:
                    _venda.SituacaoFiscalCancelado();
                    break;
                case SituacaoFiscal.AutorizadaDenegada:
                    _venda.SituacaoFiscalAutorizadoDenegada();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(situacaoFiscal), situacaoFiscal, null);
            }
        }

        private void ConsultaLoteSefaz(ConfiguracaoZeusBuilder cfg, CupomFiscalHistorico cupomFiscalHistorico)
        {
            for (var tentativa = 0; tentativa <= 3; tentativa++)
            {
                var certificado = CertificadoDigitalFactory.Cria(_emissorFiscal, true);
                var situacaoNotaSefaz = new SituacaoNotaSefaz(cfg, certificado);

                ProcessaFinalizacao(cupomFiscalHistorico, situacaoNotaSefaz);

                if (cupomFiscalHistorico.IsRejeicao999())
                {
                    throw new InvalidOperationException(cupomFiscalHistorico.GetTextoRejeicao());
                }

                if (cupomFiscalHistorico.Finalizado)
                    break;

                Thread.Sleep(5000);
            }
        }

        private static void ProcessaFinalizacao(CupomFiscalHistorico cupomFiscalHistorico, SituacaoNotaSefaz situacaoNotaSefaz)
        {
            if (cupomFiscalHistorico.PossuiRecibo())
            {
                var respostaRecibo = situacaoNotaSefaz.GetSituacaoPeloRecibo(cupomFiscalHistorico.GetRecibo());
                cupomFiscalHistorico.ProcessarRespotaLote(respostaRecibo);

                if (cupomFiscalHistorico.IsRejeicao999())
                {
                    throw new InvalidOperationException(cupomFiscalHistorico.GetTextoRejeicao());
                }

                return;
            }

            var respostaChave = situacaoNotaSefaz.GetSituacaoPelaChave(new ChaveSefaz(cupomFiscalHistorico.Chave));
            cupomFiscalHistorico.ProcessarRespostaPelaChave(respostaChave);

            if (cupomFiscalHistorico.IsRejeicao999())
            {
                throw new InvalidOperationException(cupomFiscalHistorico.GetTextoRejeicao());
            }
        }

        private void EnviarLote(CupomFiscalHistorico cupomFiscalHistorico, ConfiguracaoServico cfg)
        {
            if (cupomFiscalHistorico.FalhaEnvioLote) return;
            if (cupomFiscalHistorico.Resposta.IsNotNullOrEmpty()) return;

            try
            {
                var envio = new enviNFe4(
                    "4.00",
                    1,
                    IndicadorSincronizacao.Assincrono,
                    new List<NFe.Classes.NFe> {FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(cupomFiscalHistorico.Envio)}
                );

                var xmlEnvio = new XmlDocument();
                xmlEnvio.LoadXml(envio.ObterXmlString().RemoverAcentos());

                var wsLote = GetWsTransmissaoLote(cfg);

                var xmlRespostaLote = wsLote.Execute(xmlEnvio);
                

                if (xmlRespostaLote == null)
                {
                    throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");
                }

                Validador.Valida(xmlRespostaLote.OuterXml,
                    "retEnviNFe_v4.00.xsd",
                    new CTeOsConfig
                    {
                        CaminhoSchemas = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "Assets",
                            "Schemas.Nfe")
                    });

                cupomFiscalHistorico.ComRespostaLote(xmlRespostaLote.OuterXml);

                SalvarCupomFiscalHistorico(cupomFiscalHistorico);
            }
            catch
            {
                cupomFiscalHistorico.FalhouNoEnvioDeLote();
                SalvarCupomFiscalHistorico(cupomFiscalHistorico);
                throw;
            }
        }

        private void EnviarSincrono(CupomFiscalHistorico cupomFiscalHistorico, ConfiguracaoServico cfg)
        {
            try
            {
                var envio = new enviNFe4(
                    "4.00",
                    1,
                    IndicadorSincronizacao.Sincrono,
                    new List<NFe.Classes.NFe> { FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(cupomFiscalHistorico.Envio) }
                );

                var xmlEnvio = new XmlDocument();
                xmlEnvio.LoadXml(envio.ObterXmlString().RemoverAcentos());

                var wsLote = GetWsTransmissaoLote(cfg);

                var xmlResposta = wsLote.Execute(xmlEnvio);

                // simulação erro internet throw new WebException("simulação erro internet");
                // throw new WebException("simulação erro internet");

                if (xmlResposta == null)
                {
                    throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");
                }

                Validador.Valida(xmlResposta.OuterXml,
                    "retEnviNFe_v4.00.xsd",
                    new CTeOsConfig
                    {
                        CaminhoSchemas = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "Assets",
                            "Schemas.Nfe")
                    });

                cupomFiscalHistorico.ProcessarRespostaSincrona(xmlResposta);

                SalvarCupomFiscalHistorico(cupomFiscalHistorico);
            }
            catch
            {
                cupomFiscalHistorico.FalhouNoEnvioDeLote();
                SalvarCupomFiscalHistorico(cupomFiscalHistorico);
                throw;
            }
        }

        private static void SalvarCupomFiscalHistorico(CupomFiscalHistorico cupomFiscalHistorico)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                repositorioCupomFiscal.SalvarOuAlterarHistorico(cupomFiscalHistorico);
                transacao.Commit();
            }
        }

        private INfeServicoAutorizacao GetWsTransmissaoLote(ConfiguracaoServico cfg)
        {
            return ServicoNfeFactory.CriaWsdlAutorizacao(cfg, CertificadoDigitalFactory.Cria(_emissorFiscal, true), false);
        }

        private CupomFiscalHistorico CarregarCupomFiscalHistorico()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                return repositorioCupomFiscal.BuscarHistoricoEmAberto(_venda);
            }
        }

        private ConfiguracaoZeusBuilder CarregarConfiguracaoServicoZeus()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var emissorFiscalId = new RepositorioCupomFiscal(sessao).ObterEmissorFiscalId(_venda);
                _emissorFiscal = new RepositorioEmissorFiscal(sessao).GetPeloId(emissorFiscalId);

                return new ConfiguracaoZeusBuilder(_emissorFiscal.EmissorFiscalNfce, TipoEmissao.Normal);
            }
        }
    }
}