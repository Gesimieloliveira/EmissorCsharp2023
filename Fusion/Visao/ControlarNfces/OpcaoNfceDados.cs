using System;
using System.Collections.Generic;
using Fusion.FastReport.Facades;
using Fusion.FastReport.Facades.Infra;
using Fusion.FastReport.Relatorios.Sistema;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Repositorio;
using FusionLibrary.VisaoModel;
using FusionWPF.SendMail;
using NHibernate; 
using RepositorioPreferencia = FusionCore.Vendas.Repositorio.RepositorioPreferencia;
using LayoutImpressao = FusionCore.FusionNfce.Preferencias.LayoutImpressao;
using FusionCore.Helpers.Maquina;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.ControlarNfces
{
    public class OpcaoNfceDados : ViewModel
    {
        private readonly int _nfceId;
        private readonly bool _isFaturamento;

        public OpcaoNfceDados(int nfceId, bool isFaturamento, bool podeAutorizar)
        {
            _nfceId = nfceId;
            _isFaturamento = isFaturamento;
            PodeAutorizar = podeAutorizar;
        }

        public bool PodeAutorizar { get; }

        public void Imprimir()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var geraIdentifcador =  IdMaquinaProvider.Computa();
                var preferencia = new RepositorioPreferencia(sessao).GetPeloIdentificadorMaquina(geraIdentifcador);

                if (preferencia == null)
                {
                    throw new PreferenciaException("Configure a preferencias do faturamento");
                }

                IDadosParaImpressaoNfce dadosParaImpressaoNfce = new RepositorioNfceAdm(sessao).GetPeloId(_nfceId);

                if (_isFaturamento)
                {                    
                    var cupomFiscal = new RepositorioCupomFiscal(sessao).GetPeloId(_nfceId);
                    
                    if (cupomFiscal.IsPodeImprimir == false)
                    {
                        throw new InvalidOperationException("Cupom Fiscal deve estar: autorizado, cancelado, denegado, contingencia");
                    }

                    dadosParaImpressaoNfce = cupomFiscal;
                }

                var danfeNfceConfiguracaoDto = new DanfeNfceConfiguracaoDto();
                danfeNfceConfiguracaoDto.SemImpressora();
                danfeNfceConfiguracaoDto.ImprimirSegundaVia(false);
                danfeNfceConfiguracaoDto.PreVisualizarSomente();

                if (preferencia.LayoutImpressao == 0) 
                {
                    danfeNfceConfiguracaoDto.UsarPlanoDeImpressao(LayoutImpressao.Impressao80M);
                }
                else
                {
                    danfeNfceConfiguracaoDto.UsarPlanoDeImpressao(LayoutImpressao.ImpressaoA4);
                }
                
                danfeNfceConfiguracaoDto.SemNomeFantasiaCustomizado();
                danfeNfceConfiguracaoDto.InterromperImpressao(false);


                new DanfeNfceFacade()
                    .Imprimir(
                        danfeNfceConfiguracaoDto
                        , dadosParaImpressaoNfce
                        , new ServicoObterXml(ObterXml(sessao))
                );
            }
        }

        private IObterXml ObterXml(ISession sessao)
        {
            IObterXml obterXml = new RepositorioDanfeNfceAdm(sessao);
            if (_isFaturamento)
                obterXml = new RepositorioDanfeCupomFiscalNfce(sessao);
            return obterXml;
        }

        public string BaixarXml()
        {
            string xmlAutorizado;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                xmlAutorizado = new ServicoObterXml(ObterXml(sessao)).ObterXml(_nfceId);
            }

            return xmlAutorizado;
        }

        public void EnviaPorEmail(EnvioEmailBehavior envioEmailBehavior, IEnumerable<Email> emails)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var configuracaoEmail = new RepositorioConfiguracaoEmail(sessao).BuscarUnicaConfiguracai();
                IDadosParaEnvioEmailNfce dadosNfce = new RepositorioNfceAdm(sessao).GetPeloId(_nfceId);

                if (_isFaturamento)
                    dadosNfce = new RepositorioCupomFiscal(sessao).GetPeloId(_nfceId);

                var escreverMensagem = new EscreverMensagem(
                    envioEmailBehavior.Assunto,
                    envioEmailBehavior.CorpoMensagem,
                    string.Empty
                );

                new DanfeNfceFacade()
                    .EnviarEmail(dadosNfce, emails, escreverMensagem
                        , new RDanfeNfceA4()
                        , new ServicoObterXml(ObterXml(sessao))
                        , configuracaoEmail);
            }
        }

        public void AvancarNumeracaoFiscal()
        {
            if (_isFaturamento == false)
                throw new InvalidOperationException(
                    "Essa NFC-e foi emitida em um Terminal (PDV)\nAqui nos avançamos númeração somente para NFC-e emitidas em Faturamento");

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var cupomFiscal = new RepositorioCupomFiscal(sessao).GetPeloId(_nfceId);

                if (cupomFiscal.NumeroFiscal == 0)
                    throw new InvalidOperationException(
                        "Somente é possível alocar nova númeração fiscal quando o mesmo já tiver uma númeração diferente de 0");

                if (cupomFiscal.IsPodeAlocarNumeracao == false)
                    throw new InvalidOperationException("Cupom Fiscal deve estar: rejeição, rejeição contingência");

                var venda = new RepositorioFaturamento(sessao).GetPeloIdCompleto(cupomFiscal.Venda.Id);

                new AlocarNumeracaoCupomFiscal(venda).Aloca(sessao);


                transacao.Commit();
            }
        }

        public void ThrowPodeEnviarEmail()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                if (_isFaturamento == true)
                {
                    var cupomFiscal = new RepositorioCupomFiscal(sessao).GetPeloId(_nfceId);

                    if (cupomFiscal.IsPodeEnviarEmail == false)
                        throw new InvalidOperationException("Cupom Fiscal deve estar: autorizado, cancelado, denegado, contigência");
                }
                else
                {
                    var cupomNfce = new RepositorioNfceAdm(sessao).GetPeloId(_nfceId);

                    var status = cupomNfce.Status;

                    if (status.Equals(0) || status.Equals(3))
                    {
                        throw new InvalidOperationException("Cupom Fiscal deve estar: autorizado, cancelado, denegado, contigência");
                    }
                }
            }
        }

        public void ThrowBaixarXml()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                if (_isFaturamento == true)
                {
                    var cupomFiscal = new RepositorioCupomFiscal(sessao).GetPeloId(_nfceId);

                    if (cupomFiscal.IsPodeBaixarXml == false)
                        throw new InvalidOperationException("Cupom Fiscal deve estar: autorizado, cancelado, denegado");
                }
                else
                {
                    var cupomNfce = new RepositorioNfceAdm(sessao).GetPeloId(_nfceId);

                    var status = cupomNfce.Status;

                    if(status.Equals(0) || status.Equals(3))
                    {                       
                        throw new InvalidOperationException("Cupom Fiscal deve estar: autorizado, cancelado, denegado");
                    }
                    
                }
            }
        }

        public FaturamentoVenda BuscarFaturamentoVenda()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var cupomFiscal = new RepositorioCupomFiscal(sessao).GetPeloId(_nfceId);

                return new RepositorioFaturamento(sessao).GetPeloIdCompleto(cupomFiscal.Venda.Id);
            }
        }

        public bool EhUmFaturamento()
        {
            return _isFaturamento;
        }
    }
}