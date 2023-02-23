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
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Repositorio;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Autorizacao;
using NFe.Utils.NFe;

namespace FusionCore.Cupom.Nfce.Lotes
{
    public class HistoricoEmAndamento
    {
        public HistoricoEmAndamento(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class EnviarLoteNFCe
    {
        private readonly IList<LinhaMensagemSefaz> _linhaMensagemSefazes = new List<LinhaMensagemSefaz>();
        private readonly List<NFe.Classes.NFe> _zeusNfce = new List<NFe.Classes.NFe>();
        private readonly DateTime? _novaDataEnvio;
        private readonly IEnumerable<CupomFiscalSelecionado> _cupons;
        private int _empresaIdAtual;
        private IList<HistoricoEmAndamento> _historicoEmAndamentos = new List<HistoricoEmAndamento>();

        public EnviarLoteNFCe(DateTime? novaDataEnvio, IEnumerable<CupomFiscalSelecionado> cupons)
        {
            _novaDataEnvio = novaDataEnvio;
            _cupons = cupons;
        }

        public RespostaEnvioLote Enviar()
        {
            var cupons = BuscarCupons();

            SetarDataNovaDeEnvio(cupons);

            CriaHistoricos(cupons);

            var historicos = BuscarTodosHistoricos(cupons);

            var quantidadeMaximaDeNfce = 0;
            foreach (var cupomFiscalHistorico in historicos)
            {
                _historicoEmAndamentos.Add(new HistoricoEmAndamento(cupomFiscalHistorico.Id));
                quantidadeMaximaDeNfce++;
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

                if (tamanhoXmlBytes > 250000 || quantidadeMaximaDeNfce == 10)
                {
                    quantidadeMaximaDeNfce = 0;

                    EnviarSefaz();
                }
            }

            EnviarSefaz();

            return new RespostaEnvioLote(_linhaMensagemSefazes);
        }

        private void SetarDataNovaDeEnvio(IEnumerable<CupomFiscal> cupons)
        {
            if (_novaDataEnvio.HasValue == false) return;

            foreach (var cupomFiscal in cupons)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    cupomFiscal.AlterarEmitirEm(_novaDataEnvio.Value);

                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                    repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
                    
                    transacao.Commit();
                }
            }
        }

        private void EnviarSefaz()
        {
            if (_zeusNfce.Count == 0) return;

            var zeusConfiguracao = BuscarZeusConfiguracao();
            var servicoNFe = new ServicosNFe(zeusConfiguracao);

            var retornoEnvioLote = EnviaLoteParaSefaz(servicoNFe, _zeusNfce);

            if (retornoEnvioLote.ApenasUmaNfce())
            {
                var historico = BuscarTodosHistoricos(_cupons).FirstOrDefault();

                TratarRejeicaoApenasUmaNfce(retornoEnvioLote, historico);
                _linhaMensagemSefazes.Add(new LinhaMensagemSefaz(false, 
                    retornoEnvioLote.Motivo, string.Empty));
                return;
            }

            Thread.Sleep(30000);

            var loteRetornado = ConsultaLote(servicoNFe, retornoEnvioLote);

            TrataLote(loteRetornado);

            _zeusNfce.Clear();
            _historicoEmAndamentos.Clear();
        }

        private void TratarRejeicaoApenasUmaNfce(RespostaEnvioDeLote lote, CupomFiscalHistorico historico)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                var repositorioFaturamento = new RepositorioFaturamento(sessao);

                var cupomFiscal = repositorioCupomFiscal.GetPeloId(historico.CupomFiscal.Id);

                cupomFiscal.ComRejeicao();
                if (cupomFiscal.ContingenciaId != null)
                {
                    cupomFiscal.ComRejeicaoOffline();
                }

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

        private void TrataLote(RetornoLote loteRetornado)
        {
            loteRetornado.RetornoLoteNfes.ForEach(recibo =>
            {
                var historico = BuscarHistorico(recibo.InformacaoProtocoloResposta.Chave);

                _linhaMensagemSefazes.Add(new LinhaMensagemSefaz(
                    recibo.InformacaoProtocoloResposta.Autorizado,
                    recibo.InformacaoProtocoloResposta.Motivo,
                    recibo.InformacaoProtocoloResposta.Chave));

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

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                var repositorioFaturamento = new RepositorioFaturamento(sessao);

                var cupomFiscal = repositorioCupomFiscal.GetPeloId(historico.CupomFiscal.Id);

                cupomFiscal.ComRejeicao();
                if (cupomFiscal.ContingenciaId != null)
                {
                    cupomFiscal.ComRejeicaoOffline();
                }

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

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
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

        private CupomFiscalHistorico BuscarHistorico(string chave)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioCupomFiscal(sessao).BuscarPelaChaveNaoFinalizado(chave);
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

            var historicos = BuscarTodosHistoricos(_cupons);

            foreach (var cupomFiscalHistorico in historicos)
            {
                cupomFiscalHistorico.ComRespostaLote(retornoEnvio.RetornoCompletoStr);

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
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

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioEmissorFiscal = new RepositorioEmissorFiscal(sessao);
                var repositorioEmpresa = new RepositorioEmpresa(sessao);


                var empresaAtual = repositorioEmpresa.GetPeloId(_empresaIdAtual);
                emissorFiscal = repositorioEmissorFiscal.BuscarEmissorFaturamentoNfcePorEmpresa(empresaAtual);
            }

            return new ConfiguracaoZeusBuilder(emissorFiscal.EmissorFiscalNfce, TipoEmissao.Normal, new TimeOut(60000))
                .GetConfiguracao();
        }

        private IEnumerable<CupomFiscalHistorico> BuscarTodosHistoricos(IEnumerable<CupomFiscal> cupons)
        {
            var historicos = new List<CupomFiscalHistorico>();

            foreach (var cupomFiscal in cupons)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    historicos.Add(repositorioCupomFiscal.BuscarCupomFiscalHistoricoAberto(cupomFiscal.Id));
                }
            }

            return historicos;
        }

        private IEnumerable<CupomFiscalHistorico> BuscarTodosHistoricos(IEnumerable<CupomFiscalSelecionado> cupons)
        {
            var historicos = new List<CupomFiscalHistorico>();

            foreach (var cupomFiscal in cupons)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    var historico = repositorioCupomFiscal.BuscarCupomFiscalHistoricoAberto(cupomFiscal.Id);

                    if (historico == null) continue;

                    if (_historicoEmAndamentos.Any(h => h.Id == historico.Id))
                        historicos.Add(historico);

                }
            }

            return historicos;
        }

        private void CriaHistoricos(IEnumerable<CupomFiscal> cupons)
        {
            foreach (var cupomFiscal in cupons)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioFaturamento = new RepositorioFaturamento(sessao);

                    var criadorDeHistorico = new CupomFiscalCriaHistorico();
                    criadorDeHistorico.Criar(repositorioFaturamento.GetPeloIdCompleto(cupomFiscal.Venda.Id));
                }
            }
        }

        private IEnumerable<CupomFiscal> BuscarCupons()
        {
            IList<CupomFiscal> listaDeCupomFiscais = new List<CupomFiscal>();

            foreach (var cupomFiscalSelecionado in _cupons)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    listaDeCupomFiscais.Add(repositorioCupomFiscal.GetPeloId(cupomFiscalSelecionado.Id));
                }
            }

            _empresaIdAtual = listaDeCupomFiscais.First().Venda.Empresa.Id;

            return listaDeCupomFiscais;
        }
    }
}