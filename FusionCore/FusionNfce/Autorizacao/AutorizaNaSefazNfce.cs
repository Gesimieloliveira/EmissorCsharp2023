using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml;
using DFe.Classes.Flags;
using FusionCore.ConfiguracaoTransmissao.Nfce.Entidade;
using FusionCore.ConfiguracaoTransmissao.Nfce.Infra;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Recepcao.Retorno;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Autorizacao;
using NFe.Utils.NFe;
using NFe.Utils.Recepcao;
using NFe.Utils.Validacao;
using NFe.Wsdl;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionNfce.Autorizacao
{
    public class AutorizaNaSefazNfce
    {
        private readonly X509Certificate2 _certificado;

        public AutorizaNaSefazNfce(X509Certificate2 certificado)
        {
            _certificado = certificado;
        }

        public RespostaAutorizacao AutorizaNaSefaz(NfceEmissaoHistorico emissaoHistoico)
        {
            var cfg = new ConfiguracaoZeusBuilder(SessaoSistemaNfce.GetDadosSefaz(), TipoEmissao.Normal).GetConfiguracao();

            var zNfe = new NFe.Classes.NFe().CarregarDeXmlString(emissaoHistoico.XmlEnvio.Valor);
            const int lote = 1;

            var versaoServicoParaString = ServicoNFe.NFeAutorizacao.VersaoServicoParaString(cfg.VersaoNFeAutorizacao);

            var envio = new enviNFe4(
                versaoServicoParaString,
                lote,
                IndicadorSincronizacao.Sincrono,
                new List<NFe.Classes.NFe> {zNfe}
            );

            var transmissao = Transmissao.Sincrono;
            transmissao = SetarModoTransmissao(transmissao, envio);

            var xmlEnvio = envio.ObterXmlString().RemoverAcentos();

            ThrowExceptionSeXmlInvalido(xmlEnvio, cfg);

            var wsResultado = default(XmlNode);
            var xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(xmlEnvio);

            RespostaAutorizacao resposta = null;

            NfceEmissaoHistorico emissaoHistoricoComLote = null;

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                if (transmissao == Transmissao.Sincrono)
                {
                    var wsAutorizacao = GetWsAutorizacao(cfg);
                    wsResultado = wsAutorizacao.Execute(xmlRequest);
                }

                if (transmissao == Transmissao.Assincrono)
                {
                    emissaoHistoricoComLote = EnviaLote(emissaoHistoico, cfg, xmlRequest);

                    for (var tentativa = 0; tentativa <= 3; tentativa++)
                    {
                        wsResultado = FinalizarEmissaoAssincrona(emissaoHistoricoComLote, cfg);

                        if (wsResultado == null)
                            throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");

                        resposta = RespostaAutorizacao.Load(wsResultado);

                        if (resposta.LoteEmProcessamento)
                            continue;

                        if (resposta.Rejeicao999)
                            break;

                        if (resposta.Autorizado)
                            break;

                        if (resposta.Rejeicao)
                            break;
                    }
                }
                
            }

            resposta = RespostaAutorizacao.Load(wsResultado);

            if (resposta.Rejeicao999)
            {
                return resposta;
            }

            if (emissaoHistoricoComLote == null)
                emissaoHistoricoComLote = emissaoHistoico;

            using (var financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario))
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var historicoFinalizado = FinalizaHistoricoCorrente(emissaoHistoricoComLote, wsResultado, resposta, sessao);

                if (resposta.Autorizado)
                {
                    //Fluxo de emissão normal da nfc-e
                    financeiro.GerarFinanceiroParaNfce(emissaoHistoico.Nfce);
                    GeraRegistroCaixa(emissaoHistoico.Nfce, sessao);
                    FinalizaEmissao(emissaoHistoico.Nfce, resposta, historicoFinalizado, sessao);
                    FinalizaNfce(emissaoHistoico.Nfce, sessao);
                }

                if (resposta.Denegado)
                {
                    emissaoHistoico.Nfce.ObterTodosItens().Where(item => item.Cancelado == false).ForEach(i =>
                    {
                        var estoqueServico = EstoqueServicoNfce.Cria(
                            sessao,
                            i.Produto,
                            OrigemEventoEstoque.DenegacaoNfce,
                            TipoEventoEstoque.Entrada,
                            i.Quantidade
                        );

                        estoqueServico.Acrescentar();
                    });

                    FinalizaEmissao(emissaoHistoico.Nfce, resposta, historicoFinalizado, sessao);

                    emissaoHistoico.Nfce.Denegada = true;
                    FinalizaNfce(emissaoHistoico.Nfce, sessao);
                }

                financeiro.ComitarAlteracoes();
                transacao.Commit();
            }

            return resposta;
        }

        private NfceEmissaoHistorico EnviaLote(NfceEmissaoHistorico emissaoHistoico, ConfiguracaoServico cfg, XmlDocument xmlRequest)
        {
            try
            {
                var wsLote = GetWsTransmissaoLote(cfg);
                var wsResultado = wsLote.Execute(xmlRequest);

                if (wsResultado == null)
                {
                    throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");
                }

                var emissaoHistoricoComLote  = emissaoHistoico.ToBuilder().ComXmlLote(wsResultado.OuterXml);

                SalvarEmissaoHistorico(emissaoHistoricoComLote);

                return emissaoHistoricoComLote;
            }
            catch
            {
                var emissaoHistoricoComLote = emissaoHistoico.ToBuilder().ComFalhaReceberLote(true);

                SalvarEmissaoHistorico(emissaoHistoricoComLote);
                throw;
            }
        }

        private XmlNode FinalizarEmissaoAssincrona(NfceEmissaoHistorico emissaoHistoico,
            ConfiguracaoServico configuracaoServico)
        {
            Thread.Sleep(5000);

            var versaoServico = ServicoNFe.NFeRetAutorizacao.VersaoServicoParaString(configuracaoServico.VersaoNFeRetAutorizacao);
            var wsConsultaRecibo = ServicoNfeFactory.CriaWsdlOutros(ServicoNFe.NFeRetAutorizacao, configuracaoServico, _certificado);

            var pedRecibo = new consReciNFe
            {
                versao = versaoServico,
                tpAmb = configuracaoServico.tpAmb,
                nRec = emissaoHistoico.ObterRecibo()
            };

            var xmlEnvio = new XmlDocument();
            xmlEnvio.LoadXml(pedRecibo.ObterXmlString());

            var xmlResultado =  wsConsultaRecibo.Execute(xmlEnvio);

            return xmlResultado;
        }

        private void SalvarEmissaoHistorico(NfceEmissaoHistorico emissaoHistoico)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);
                repositorioNfce.SalvarHistorico(emissaoHistoico);

                transacao.Commit();
            }
        }

        private static Transmissao SetarModoTransmissao(Transmissao transmissao, enviNFe4 envio)
        {
            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
                {
                    IRepositorioConfiguracaoTransmissaoNfce repositorioAutorizacao =
                        new RepositorioConfiguracaoTransmissao(sessao);
                    transmissao = repositorioAutorizacao.ObterModoTransmissao();
                }

                envio.indSinc = transmissao == Transmissao.Sincrono
                    ? IndicadorSincronizacao.Sincrono
                    : IndicadorSincronizacao.Assincrono;
            }

            return transmissao;
        }

        private void GeraRegistroCaixa(Nfce nfce, ISession sessao)
        {
            new GeraRegistroCaixa(nfce, sessao, SessaoSistemaNfce.Usuario).RegistrarCaixa();
        }

        private void FinalizaNfce(Nfce nfce, ISession sessao)
        {
            var repositorioNfce = new RepositorioNfce(sessao);
            nfce.TransmitidaComSucesso();
            repositorioNfce.SalvarESincronizar(nfce);
        }

        private void FinalizaEmissao(Nfce nfce, RespostaAutorizacao resposta, NfceEmissaoHistorico historicoFinalizado, ISession sessao)
        {
            nfce.Emissao = new NfceEmissao(nfce, nfce.NumeroFiscal, nfce.Serie, historicoFinalizado.AmbienteSefaz);
            var emissao = nfce.Emissao;

            emissao.TipoEmissao = SessaoSistemaNfce.TipoEmissao;
            emissao.TipoAmbiente = historicoFinalizado.AmbienteSefaz;
            emissao.VersaoAplicativo = ResponsavelLegal.VersaoSistema;
            emissao.TagId = $"NFe{historicoFinalizado.ChaveTexto.Valor}";
            emissao.Chave = historicoFinalizado.ChaveTexto.Valor;
            emissao.CodigoNumerico = nfce.CodigoNumerico;
            emissao.EmissorFiscal = SessaoSistemaNfce.Configuracao.EmissorFiscal;
            emissao.Autorizado = true;
            emissao.CodigoAutorizacao = resposta.CodigoAutorizacao;
            emissao.RecebidoEm = resposta.DataHoraRecebimento;
            emissao.Chave = historicoFinalizado.ChaveTexto.Valor;
            emissao.VersaoAplicativoAutorizacao = resposta.VersaoAplicacao ?? string.Empty;
            emissao.DigestValue = resposta.DigestValue ?? string.Empty;
            emissao.Protocolo = resposta.NumeroProtocolo ?? string.Empty;
            emissao.CodigoNumerico = historicoFinalizado.Chave.CodigoNumerico.Valor;
            emissao.EntrouEmContingenciaEm = historicoFinalizado.Contingencia.EntrouEm;
            emissao.JustificativaContingencia = historicoFinalizado.Contingencia.Justificativa;
            emissao.AutorizaXml(historicoFinalizado);

            var repositorioNfce = new RepositorioNfce(sessao);
            repositorioNfce.SalvarEmissao(emissao);
        }

        private static NfceEmissaoHistorico FinalizaHistoricoCorrente(NfceEmissaoHistorico emissaoHistoico, XmlNode wsResultado, RespostaAutorizacao resposta, ISession sessao)
        {
                var emissaoHistoricoFinalizado = emissaoHistoico.ToBuilder()
                    .ComXmlDeRetorno(new XmlRetorno(wsResultado.OuterXml))
                    .ComCodigoDeAutorizacao(new CodigoAutorizacao(resposta.CodigoAutorizacao))
                    .ComMotivo(new Motivo(resposta.TextoAutorizacao ?? string.Empty))
                    .Finalizar();


                var repositorioNfce = new RepositorioNfce(sessao);
                repositorioNfce.SalvarHistorico(emissaoHistoricoFinalizado);

                return emissaoHistoricoFinalizado;
        }

        private static void ThrowExceptionSeXmlInvalido(string xmlEnvio, ConfiguracaoServico cfg)
        {
            ConfiguracaoServico.Instancia.DiretorioSchemas = cfg.DiretorioSchemas;

            Validador.Valida(ServicoNFe.NFeAutorizacao, cfg.VersaoNFeAutorizacao, xmlEnvio, true, cfg);
        }

        private INfeServicoAutorizacao GetWsAutorizacao(ConfiguracaoServico cfg)
        {
            return ServicoNfeFactory.CriaWsdlAutorizacao(cfg, _certificado, false);
        }

        private INfeServicoAutorizacao GetWsTransmissaoLote(ConfiguracaoServico cfg)
        {
            return ServicoNfeFactory.CriaWsdlAutorizacao(cfg, _certificado, false);
        }
    }
}