using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFe.Utils;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Contratos.EnviaLote;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Vendas.Autorizadores.Nfce;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;
using NFe.Utils.Autorizacao;
using NFe.Utils.NFe;
using NHibernate.Util;
using ZeusNfe = NFe.Classes.NFe;


namespace FusionCore.FusionAdm.Fiscal.NF.EnviaLote
{
    public class GrupoLote
    {
        public int CodigoAutorizacao { get; set; }
        public string Mensagem { get; set; }
        public int Quantidade { get; set; }
    }

    public class EnviarNfceEmLotesZeus : IEnviarNfceEmLotes
    {
        private readonly IList<FusionNfce.Fiscal.Nfce> _listaNfce;
        private readonly List<ZeusNfe> _nfZeus = new List<ZeusNfe>();
        private readonly ConfiguracaoServico _configuracaoZeus400;
        public List<FusionNfce.Fiscal.Nfce> NfceNaoEmitida { get; } = new List<FusionNfce.Fiscal.Nfce>();
        public List<GrupoLote> GrupoLotes { get; set; }
        public System.Exception UltimaException { get; set; }

        public EnviarNfceEmLotesZeus(IList<FusionNfce.Fiscal.Nfce> listaDeNfces, IDadosServicoSefaz dadosServico)
        {
            _listaNfce = listaDeNfces;
            _configuracaoZeus400 = new ConfiguracaoZeusBuilder(dadosServico, TipoEmissao.Normal).GetConfiguracao();
        }

        public async Task EnviaAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    FazEnvioParaSefaz();
                }
                catch (System.Exception ex)
                {
                    UltimaException = ex;
                }
            });
        }

        private void FazEnvioParaSefaz()
        {
            var quantidadeMaximaDeNfe = 0;

            _listaNfce.ForEach(nfce =>
            {
                quantidadeMaximaDeNfe++;

                string xmlstring = null;
                
                using (var repositorio = new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao()))
                {
                    xmlstring = repositorio.BuscarUltimoXmlAssinado(nfce.Id);
                }

                if (xmlstring == null)
                {
                    nfce.PendenteOfflineRejeitada();

                    using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
                    using (var transacao = sessao.BeginTransaction())
                    {
                        var repositorioNfce = new RepositorioNfce(sessao);

                        repositorioNfce.Salvar(nfce);

                        transacao.Commit();
                    }
                }

                if (xmlstring != null)
                {
                    var nfeZeus = new ZeusNfe().CarregarDeXmlString(xmlstring);
                    var tamanhoXmlBytes = 0;

                    var versaoServico = ServicoNFe.NFeAutorizacao.VersaoServicoParaString(_configuracaoZeus400.VersaoNFeAutorizacao);

                    _nfZeus.Add(nfeZeus);
                    var xmlEnvio = new enviNFe4(versaoServico, Convert.ToInt32(1), IndicadorSincronizacao.Assincrono, _nfZeus)
                        .ObterXmlString();

                    tamanhoXmlBytes += Encoding.Unicode.GetByteCount(xmlEnvio);

                    if (tamanhoXmlBytes > 250000 || quantidadeMaximaDeNfe == 10)
                    {
                        quantidadeMaximaDeNfe = 0;

                        EfetuaEnvio();
                    }
                }
            });

            EfetuaEnvio();
        }

        private void EfetuaEnvio()
        {
            if (_nfZeus.Count == 0) return;

            var servicoNFe = new ServicosNFe(_configuracaoZeus400);
            var retornoEnvioLote = EnviaLoteParaSefaz(servicoNFe, _nfZeus);

            if (ApenasUmaNFCe(retornoEnvioLote)) return;

            SalvaLoteNoBancoDeDados(retornoEnvioLote);

            Thread.Sleep(30000);

            var loteRetornado = ConsultaLote(servicoNFe, retornoEnvioLote);

            TrataLote(loteRetornado);

            MontaErrosSeOuver(loteRetornado);

            _nfZeus.Clear();
        }

        private bool ApenasUmaNFCe(RespostaEnvioDeLote retornoEnvioLote)
        {
            if (retornoEnvioLote.ApenasUmaNfce() == false) return false;

            var chave = _nfZeus[0].ObterChaveNfeZeus();
            var historico = BuscarHistorico(chave);
            var nfce = historico.Nfce;

            nfce.PendenteOfflineRejeitada();

            var novoHistorico = historico.ToBuilder(true)
                .ComCodigoDeAutorizacao(new CodigoAutorizacao(short.Parse(retornoEnvioLote.CodigoStatus.ToString())))
                .ComMotivo(new Motivo(retornoEnvioLote.Motivo))
                .ComXmlDeRetorno(new XmlRetorno(FuncoesXml.ClasseParaXmlString(retornoEnvioLote)))
                .Finalizar();

            FinalizaRejeicao(nfce, novoHistorico);

            return true;

        }

        private void MontaErrosSeOuver(RetornoLote loteRetornado)
        {
            GrupoLotes = loteRetornado.RetornoLoteNfes
                .Where(r =>
                    r.InformacaoProtocoloResposta.CodigoStatus != 100)
                .GroupBy(x => new {x.InformacaoProtocoloResposta.CodigoStatus, x.InformacaoProtocoloResposta.Motivo})
                .Select(y => new GrupoLote
                {
                    CodigoAutorizacao = y.Key.CodigoStatus,
                    Mensagem = y.Key.Motivo,
                    Quantidade = y.ToList().Count
                }).ToList();
        }

        private void TrataLote(RetornoLote loteRetornado)
        {
            foreach (var recibo in loteRetornado.RetornoLoteNfes)
            {
                var historico = BuscarHistorico(recibo.InformacaoProtocoloResposta.Chave);

                if (recibo.InformacaoProtocoloResposta.Autorizado == false)
                {
                    TrataRejeicoes(recibo, historico);
                    NfceNaoEmitida.Add(historico.Nfce);
                    continue;
                }

                TrataAutorizacaoComSucesso(recibo, historico);
            }
        }

        private NfceEmissaoHistorico BuscarHistorico(string chaveAcesso)
        {
            NfceEmissaoHistorico emissaoHistorico;

            using (var repositorio = new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao()))
            {
                emissaoHistorico = repositorio.BuscarHistoricoPelaChaveDeAcesso(chaveAcesso);
            }

            return emissaoHistorico;
        }

        private void TrataRejeicoes(ProtocoloRecebimentoNfe recibo, NfceEmissaoHistorico historico)
        {
            var nfce = historico.Nfce;

            nfce.PendenteOfflineRejeitada();

            var novoHistorico = historico.ToBuilder(true).ComCodigoDeAutorizacao(new CodigoAutorizacao(short.Parse(recibo.InformacaoProtocoloResposta.CodigoStatus.ToString())))
                .ComMotivo(new Motivo(recibo.InformacaoProtocoloResposta.Motivo))
                .ComXmlDeRetorno(new XmlRetorno(FuncoesXml.ClasseParaXmlString(recibo)))
                .Finalizar();

            FinalizaRejeicao(nfce, novoHistorico);
        }

        private void FinalizaRejeicao(FusionNfce.Fiscal.Nfce nfce, NfceEmissaoHistorico historico)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);

                repositorioNfce.SalvarESincronizar(nfce);
                repositorioNfce.SalvarHistorico(historico);

                transacao.Commit();
            }
        }

        private void TrataAutorizacaoComSucesso(ProtocoloRecebimentoNfe recibo, NfceEmissaoHistorico emissaoHistorico)
        {
            var nfce = emissaoHistorico.Nfce;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var configuracao = new ServicoProcessaFinalizacaoNFCe.Configuracao(
                    new RepositorioNfce(sessao), emissaoHistorico,
                    nfce, recibo);

                var servicoFinalizacao = new ServicoProcessaFinalizacaoNFCe(configuracao);
                servicoFinalizacao.Finaliza();

                transacao.Commit();
            }
        }


        private RespostaEnvioDeLote EnviaLoteParaSefaz(ServicosNFe servicoNFe, List<ZeusNfe> listaZeus)
        {
            RetornoNFeAutorizacao retornoEnvio = null;

            retornoEnvio = servicoNFe.NFeAutorizacao(Convert.ToInt32(1), IndicadorSincronizacao.Assincrono, listaZeus);

            var retornoEnvioLote = RespostaEnvioDeLote.Carregar(retornoEnvio.RetornoCompletoStr);

            return retornoEnvioLote;
        }
        private static RetornoLote ConsultaLote(ServicosNFe servicoNFe, RespostaEnvioDeLote retornoEnvioLote)
        {
            var retornoRecibo = servicoNFe.NFeRetAutorizacao(retornoEnvioLote.DadosRecebidoDoLote.NumeroRecibo);

            var loteRetornado = RetornoLote.Carregar(retornoRecibo.RetornoCompletoStr);
            return loteRetornado;
        }
        private void SalvarLote(NfceEnvioLote nfceEnvioLote)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfce(sessao);


                repositorio.SalvarNfceEnvioLote(nfceEnvioLote);
                transacao.Commit();
            }
        }
        private void SalvaLoteNoBancoDeDados(RespostaEnvioDeLote retornoEnvioLote)
        {
            var nfceEnvioLote = new NfceEnvioLote
            {
                Ambiente = (TipoAmbiente)retornoEnvioLote.Ambiente,
                CodigoStatus = retornoEnvioLote.CodigoStatus,
                CodigoUf = retornoEnvioLote.CodigoUf,
                DataEHoraDoProcessamento = retornoEnvioLote.DataEHoraDoProcessamento,
                Motivo = retornoEnvioLote.Motivo,
                NumeroRecibo = retornoEnvioLote.DadosRecebidoDoLote.NumeroRecibo,
                TempoMedio = retornoEnvioLote.DadosRecebidoDoLote.TempoMedioResposta,
                VersaoAplicacao = retornoEnvioLote.VersaoAplicacao,
                ComErro = false
            };

            SalvarLote(nfceEnvioLote);
        }
    }
}