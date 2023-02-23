using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DFe.Classes.Entidades;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using log4net;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;

// ReSharper disable InconsistentNaming

namespace FusionCore.FusionAdm.Fiscal.NF.CCe
{
    public sealed class EmissorCCeSefaz
    {
        private const int COD_SUCESSO_EVENTO = 135;
        private const int COD_DUPLICIDADE_EVENTO = 573;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmissorFiscalNFE _emissorFiscal;
        private readonly ISessaoManager _sessaoManager;
        private readonly string _chaveNFe;
        private ConfiguracaoServico _zeusCfg;

        public EmissorCCeSefaz(EmissorFiscalNFE emissorFiscal, ISessaoManager sessaoManager, string chaveNFe)
        {
            _emissorFiscal = emissorFiscal;
            _sessaoManager = sessaoManager;
            _chaveNFe = chaveNFe;
        }

        public event EventHandler<CartaCorrecaoNfe> Sucesso;
        public event EventHandler<FalhaEmissaoCce> Falha;

        private void OnSucesso(CartaCorrecaoNfe e)
        {
            Sucesso?.Invoke(this, e);
        }

        private void OnFalha(FalhaEmissaoCce e)
        {
            Falha?.Invoke(this, e);
        }

        public void ConfigurarServico()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioContingenciaNfe(sessao);
                var contingenciaAtiva = repositorio.ContingenciaAberta(_emissorFiscal.EmissorFiscal);
                var tipoEmissao = contingenciaAtiva?.TipoEmissao ?? TipoEmissao.Normal;

                _zeusCfg = new ConfiguracaoZeusBuilder(_emissorFiscal, tipoEmissao).GetConfiguracao();
            }
        }

        public void EmitirCartaCorrecaoAsync(SolicitacaoCCe solicitacao)
        {
            Task.Run(() =>
            {
                RetornoRecepcaoEvento resposta = null;

                try
                {
                    if (solicitacao.NfeFoiEmitida == false)
                        throw new InvalidOperationException("NFe não foi emtida!");

                    if (_zeusCfg.cUF != (Estado) solicitacao.UfEmissaoNfe)
                        throw new InvalidOperationException("Nfe não foi emitida no mesmo estado do certificado!");

                    if (_zeusCfg.tpAmb != solicitacao.AmbienteNfe.ToZeus())
                        throw new InvalidOperationException("Nfe não foi emitida no mesmo ambiente do certificado");

                    var sequenciaEvento = GetSequenciaEvento(solicitacao.Nfe);
                    resposta = EnviaSolicitacaoCceParaSefaz(solicitacao, sequenciaEvento);

                    switch (resposta.Retorno.retEvento[0].infEvento.cStat)
                    {
                        case COD_DUPLICIDADE_EVENTO:
                            TrataDuplicidade573(solicitacao, resposta, sequenciaEvento);
                            return;
                        case COD_SUCESSO_EVENTO:
                            ProcessaRespostaComSucesso(resposta, solicitacao);
                            return;
                    }

                    ProcessaRespostaComFalha(resposta);
                }
                catch (System.Exception e)
                {
                    _log.Error(e);
                    _log.Info("Resposta Sefaz (Carta Correção): " +
                              (resposta?.RetornoCompletoStr ?? "Não possui retorno sefaz"));

                    OnFalha(new FalhaEmissaoCce("Erro ao processar: " + e.Message));
                }
            });
        }

        private void TrataDuplicidade573(SolicitacaoCCe solicitacao,
            RetornoRecepcaoEvento resposta,
            int sequenciaEvento)
        {
            var servicoNFe = new ServicosNFe(_zeusCfg);

            var chaveNfe = resposta.Retorno.retEvento[0].infEvento.chNFe ?? _chaveNFe;

            var retornoConsulta =
                servicoNFe.NfeConsultaProtocolo(chaveNfe);

            var evento =
                retornoConsulta.Retorno.procEventoNFe.SingleOrDefault(p => p.evento.infEvento.chNFe == chaveNfe
                                                                           && p.evento.infEvento.tpEvento == NFeTipoEvento.TeNfeCartaCorrecao
                                                                           && p.evento.infEvento.nSeqEvento ==
                                                                           sequenciaEvento);

            resposta.Retorno.retEvento[0].infEvento = evento.retEvento.infEvento;

            ProcessaRespostaComSucesso(resposta, solicitacao);
        }

        private int GetSequenciaEvento(Nfeletronica nfe)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioCce = new RepositorioCCe(sessao);
                var cces = repositorioCce.BuscaPelaNfe(nfe);

                var ultimo = cces.LastOrDefault();

                return (ultimo?.SequenciaEvento ?? 0) + 1;
            }
        }

        private RetornoRecepcaoEvento EnviaSolicitacaoCceParaSefaz(SolicitacaoCCe solicitacao, int sequencia)
        {
            var servico = new ServicosNFe(_zeusCfg);
            var retorno = servico.RecepcaoEventoCartaCorrecao(1,
                sequencia,
                solicitacao.ChaveNfe,
                solicitacao.Correcao,
                solicitacao.DocumentoUnico.ToString());

            return retorno;
        }

        private void ProcessaRespostaComSucesso(RetornoRecepcaoEvento resposta, SolicitacaoCCe solicitacao)
        {
            CartaCorrecaoNfe cce;

            try
            {
                using (var sessao = _sessaoManager.CriaSessao())
                {
                    var repostorio = new RepositorioCCe(sessao);

                    cce = solicitacao.CriaCce();
                    cce.Protocolo = resposta.Retorno.retEvento[0].infEvento.nProt;
                    cce.StatusRetorno = resposta.Retorno.retEvento[0].infEvento.cStat;
                    cce.XmlEnvio = resposta.EnvioStr;
                    cce.XmlRetorno = resposta.RetornoCompletoStr;
                    cce.SequenciaEvento = (byte) (resposta.Retorno.retEvento[0].infEvento.nSeqEvento ?? 0);

                    repostorio.Persistir(cce);
                }
            }
            catch (System.Exception e)
            {
                var falha = new FalhaEmissaoCce("Erro ao salvar resposta da CC-e no banco de dados: " + e.Message);
                OnFalha(falha);
                return;
            }

            OnSucesso(cce);
        }

        private void ProcessaRespostaComFalha(RetornoRecepcaoEvento resposta)
        {
            var falha = new FalhaEmissaoCce(resposta);
            OnFalha(falha);
        }
    }
}