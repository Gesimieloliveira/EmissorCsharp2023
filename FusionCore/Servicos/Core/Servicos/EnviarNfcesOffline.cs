using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using DFe.Utils;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.EnviaLote;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Repositorio;
using FusionCore.Vendas.Servicos;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Autorizacao;
using NFe.Utils.NFe;

namespace FusionCore.Servicos.Core.Servicos
{
    public class EnviarNfcesOffline
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly List<NFe.Classes.NFe> _zeusNfce = new List<NFe.Classes.NFe>();
        private int _empresaIdAtual;

        public EnviarNfcesOffline(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void Enviar()
        {
            if (ContingenciaAtiva.EstaAtiva())
            {
                return;
            }

            var nfces = ObterHistoricos();

            var quantidadeMaximaDeNfe = 0;
            foreach (var cupomFiscalHistorico in nfces)
            {
                quantidadeMaximaDeNfe++;

                var nfeZeus = new NFe.Classes.NFe().CarregarDeXmlString(cupomFiscalHistorico.Envio);
                _zeusNfce.Add(nfeZeus);

                var xmlEnvio = string.Empty;
                var tamanhoXmlBytes = 0;

                xmlEnvio =
                    new enviNFe4("4.00", Convert.ToInt32(1)
                            , IndicadorSincronizacao.Assincrono
                            , new List<NFe.Classes.NFe> { nfeZeus })
                        .ObterXmlString();

                tamanhoXmlBytes += Encoding.Unicode.GetByteCount(xmlEnvio);

                if (tamanhoXmlBytes > 250000 || quantidadeMaximaDeNfe == 10)
                {
                    quantidadeMaximaDeNfe = 0;

                    EnviarSefaz();
                }
            }

            EnviarSefaz();
        }

        private void EnviarSefaz()
        {
            if (_zeusNfce.Count == 0) return;

            var zeusConfiguracao = BuscarZeusConfiguracao();

            

            var servicoNFe = new ServicosNFe(zeusConfiguracao);
            var retornoEnvioLote = EnviaLoteParaSefaz(servicoNFe, _zeusNfce);

            if (retornoEnvioLote.ApenasUmaNfce())
            {
                var historico = ObterHistoricos().FirstOrDefault();

                TratarRejeicaoApenasUmaNfce(retornoEnvioLote, historico);
                return;
            }

            Thread.Sleep(30000);

            var loteRetornado = ConsultaLote(servicoNFe, retornoEnvioLote);

            TrataLote(loteRetornado);

            _zeusNfce.Clear();
        }

        private void TrataLote(RetornoLote loteRetornado)
        {
            loteRetornado.RetornoLoteNfes.ForEach(recibo =>
            {
                var historico = BuscarHistorico(recibo.InformacaoProtocoloResposta.Chave);


                var loteIndividual = new RetornoLote
                {
                    RetornoLoteNfes = new List<ProtocoloRecebimentoNfe> { recibo },
                    Ambiente = loteRetornado.Ambiente,
                    CodigoStatus = loteRetornado.CodigoStatus,
                    CodigoUf = loteRetornado.CodigoUf,
                    Motivo = loteRetornado.Motivo,
                    NumeroReciboLote = loteRetornado.NumeroReciboLote,
                    VersaoAplicacao = loteRetornado.VersaoAplicacao
                };

                TrataAutorizacaoComSucesso(loteIndividual, historico);

                TrataRejeicoes(loteIndividual, historico);
            });
        }

        private void TrataRejeicoes(RetornoLote lote, CupomFiscalHistorico historico)
        {
            if (lote.RetornoLoteNfes[0].InformacaoProtocoloResposta.Autorizado) return;

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                var repositorioFaturamento = new RepositorioFaturamento(sessao);

                var cupomFiscal = repositorioCupomFiscal.GetPeloId(historico.CupomFiscal.Id);
                cupomFiscal.ComRejeicaoOffline();


                var xmlNode = new XmlDocument();
                xmlNode.LoadXml(FuncoesXml.ClasseParaXmlString(lote));
                historico.ProcessarRespotaLote(xmlNode);
                historico.NaoFinalizado();

                var venda = cupomFiscal.Venda;
                venda.SituacaoFiscalRejeicaoOffline();


                repositorioCupomFiscal.SalvarOuAlterarHistorico(historico);
                repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
                repositorioFaturamento.Salvar(venda);

                transacao.Commit();
            }
        }

        private void TrataAutorizacaoComSucesso(RetornoLote lote, CupomFiscalHistorico historico)
        {
            if (!lote.RetornoLoteNfes[0].InformacaoProtocoloResposta.Autorizado) return;

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                var repositorioFaturamento = new RepositorioFaturamento(sessao);

                var cupomFiscal = repositorioCupomFiscal.GetPeloId(historico.CupomFiscal.Id);
                cupomFiscal.Autorizada();
                historico.FoiFinalizado();

                var xmlNode = new XmlDocument();
                xmlNode.LoadXml(FuncoesXml.ClasseParaXmlString(lote));

                historico.ProcessarRespotaLote(xmlNode);
                historico.MontarXmlProcessado();

                cupomFiscal.ComCupomFinalizado(new CupomFiscalFinalizado(
                    cupomFiscal, historico.Chave, historico.ObterProtocolo()
                    , historico.ObterDataReciboEm()
                    , historico.MontarXmlProcessado()
                ));

                var venda = cupomFiscal.Venda;
                venda.SituacaoFiscalAutorizado();

                repositorioCupomFiscal.SalvarOuAlterarHistorico(historico);
                repositorioCupomFiscal.SalvarFinalizacao(cupomFiscal.CupomFiscalFinalizado);
                repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
                repositorioFaturamento.Salvar(venda);

                transacao.Commit();
            }
        }

        private void TratarRejeicaoApenasUmaNfce(RespostaEnvioDeLote lote, CupomFiscalHistorico historico)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                var repositorioFaturamento = new RepositorioFaturamento(sessao);

                var cupomFiscal = repositorioCupomFiscal.GetPeloId(historico.CupomFiscal.Id);
                cupomFiscal.ComRejeicaoOffline();

                var xmlNode = new XmlDocument();
                xmlNode.LoadXml(FuncoesXml.ClasseParaXmlString(lote));
                historico.ProcessarRespotaLote(xmlNode);
                historico.NaoFinalizado();

                var venda = cupomFiscal.Venda;
                venda.SituacaoFiscalComRejeicao();
                if (cupomFiscal.ContingenciaId != null)
                {
                    venda.SituacaoFiscalRejeicaoOffline();
                }

                repositorioCupomFiscal.SalvarOuAlterarHistorico(historico);
                repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
                repositorioFaturamento.Salvar(venda);

                transacao.Commit();
            }
        }

        private CupomFiscalHistorico BuscarHistorico(string chave)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                return new RepositorioCupomFiscal(sessao).BuscarPelaChave(chave);
            }
        }

        private RetornoLote ConsultaLote(ServicosNFe servicoNFe, RespostaEnvioDeLote retornoEnvioLote)
        {
            var retornoRecibo = servicoNFe.NFeRetAutorizacao(retornoEnvioLote.DadosRecebidoDoLote.NumeroRecibo);

            var loteRetornado = RetornoLote.Carregar(retornoRecibo.RetornoCompletoStr);
            return loteRetornado;
        }

        private RespostaEnvioDeLote EnviaLoteParaSefaz(ServicosNFe servicoNFe, List<NFe.Classes.NFe> zeusNfce)
        {
            var retornoEnvio = servicoNFe.NFeAutorizacao(Convert.ToInt32(1), IndicadorSincronizacao.Assincrono, zeusNfce);

            var retornoEnvioLote = RespostaEnvioDeLote.Carregar(retornoEnvio.RetornoCompletoStr);

            var historicos = ObterHistoricos();

            foreach (var cupomFiscalHistorico in historicos)
            {
                cupomFiscalHistorico.ComRespostaLote(retornoEnvio.RetornoCompletoStr);

                using (var sessao = _sessaoManager.CriaSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                    repositorioCupomFiscal.SalvarOuAlterarHistorico(cupomFiscalHistorico);

                    transacao.Commit();
                }
            }

            return retornoEnvioLote;
        }

        private ConfiguracaoServico BuscarZeusConfiguracao()
        {
            EmissorFiscal emissorFiscal = null;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioEmissorFiscal = new RepositorioEmissorFiscal(sessao);
                var repositorioEmpresa = new RepositorioEmpresa(sessao);


                var empresaAtual = repositorioEmpresa.GetPeloId(_empresaIdAtual);
                emissorFiscal = repositorioEmissorFiscal.BuscarEmissorFaturamentoNfcePorEmpresa(empresaAtual);
            }

            return new ConfiguracaoZeusBuilder(emissorFiscal.EmissorFiscalNfce, TipoEmissao.Normal, new TimeOut(60000))
                .GetConfiguracao();
        }

        private IEnumerable<CupomFiscalHistorico> ObterHistoricos()
        {
            IList<CupomFiscal> cuponsFiscais = null;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                cuponsFiscais = new RepositorioCupomFiscal(sessao).BuscarCuponsOffline();

                if (cuponsFiscais.Count != 0)
                {
                    var ids = cuponsFiscais.GroupBy(x => x.Venda.Empresa.Id).Select(x => x.Key).ToList();

                    _empresaIdAtual = ids.First();
                    cuponsFiscais = cuponsFiscais.Where(x => x.Venda.Empresa.Id == _empresaIdAtual).ToList();
                }
            }

            var nfces = new List<CupomFiscalHistorico>();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                foreach (var cuponsFiscal in cuponsFiscais)
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                    nfces.Add(repositorioCupomFiscal.BuscarCupomFiscalHistoricoUnico(cuponsFiscal.Id));
                }
            }

            return nfces;
        }
    }
}