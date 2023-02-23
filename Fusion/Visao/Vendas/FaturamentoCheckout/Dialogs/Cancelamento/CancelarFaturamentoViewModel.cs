using System;
using System.Linq;
using System.Net;
using Fusion.Sessao;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF.InutilizacaoNumero;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Faturamentos;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Cancelamento
{
    public class CancelarFaturamentoViewModel : ViewModel
    {
        private readonly FaturamentoVenda _venda;

        public CancelarFaturamentoViewModel(FaturamentoVenda venda)
        {
            _venda = venda;
        }

        private string _justificativa;
        private CupomFiscal _cupomFiscal;
        private EmissorFiscalNFCE _emissorFiscalNfce;

        public string Justificativa
        {
            get => _justificativa;
            set
            {
                _justificativa = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler CanceladoSucesso;

        public void Iniciar()
        {
            CarregarCupomFiscal();
            CarregarEmissorFiscal();
        }

        public void Cancelar()
        {
            if (Justificativa.IsNullOrEmpty() || Justificativa.Length <= 14)
                throw new InvalidOperationException("Justificativa deve ter no mínimo 15 caracteres");

            NfeInutilizacaoNumeracaoDTO inutilizacaoDto = null;

            if (_cupomFiscal != null
                && _cupomFiscal.CupomFiscalFinalizado.IsNull()
                && _cupomFiscal.NumeroFiscal != 0)
            {
                var inutilizacaoZeus = CriaInutilizadorServicoZeus();

                var sucesso = inutilizacaoZeus.EnviarParaSefaz();

                inutilizacaoDto = new NfeInutilizacaoNumeracaoDTO
                {
                    Justificativa = sucesso.Inutilizacao.Justificativa,
                    Serie = sucesso.Inutilizacao.Serie,
                    Protocolo = sucesso.Protocolo,
                    Ano = sucesso.Inutilizacao.Ano,
                    CnpjEmitente = sucesso.Inutilizacao.CnpjEmitente,
                    CodigoUfSolicitante = _venda.Empresa.EstadoDTO.CodigoIbge,
                    InutilizacaoEm = DateTime.Now,
                    ModeloDocumento = ModeloDocumento.NFCe,
                    NumeroFinal = _cupomFiscal.NumeroFiscal,
                    NumeroInicial = _cupomFiscal.NumeroFiscal,
                    Uuid = GuuidHelper.Computar()
                };
            }

            if (_cupomFiscal != null && _cupomFiscal.CupomFiscalFinalizado.IsNotNull())
            {
                var configuracao = ObterConfiguracaoZeus();

                using (var servico = new ServicosNFe(configuracao))
                {
                    VerificaCancelamentoSefaz(servico);

                    if (_cupomFiscal.NaoEstaCancelada())
                        SolicitaCancelamento(servico);
                }

                _venda.SituacaoFiscalCancelado();
            }

            if (_cupomFiscal != null)
            {
                _cupomFiscal.CanceladoSemXml();
            }

            SalvaOperacao(inutilizacaoDto);
        }

        private NfeInutilizacaoZeus CriaInutilizadorServicoZeus()
        {
            var ano = byte.Parse(DateTime.Now.Year.ToString().Substring(2, 2));
            var inutilizacaoZeus = new NfeInutilizacaoZeus(
                ano,
                _cupomFiscal.Serie,
                _cupomFiscal.NumeroFiscal,
                _cupomFiscal.NumeroFiscal,
                "Inutilizado por cancelamento de faturamento nao transmitido",
                _venda.Empresa.DocumentoUnico
            );

            inutilizacaoZeus.DadosServicoSefaz = _emissorFiscalNfce;
            return inutilizacaoZeus;
        }

        private ConfiguracaoServico ObterConfiguracaoZeus()
        {
            var cfgBuilder = new ConfiguracaoZeusBuilder(_emissorFiscalNfce, _cupomFiscal.TipoEmissao);
            var configuracao = cfgBuilder.GetConfiguracao();
            return configuracao;
        }

        private void VerificaCancelamentoSefaz(ServicosNFe servico)
        {
            try
            {
                var chave = _cupomFiscal.CupomFiscalFinalizado.Chave;

                var consulta = servico.NfeConsultaProtocolo(chave);

                var evento = consulta.Retorno.procEventoNFe
                    .SingleOrDefault(i => i.evento.infEvento.chNFe == chave && i.evento.infEvento.tpEvento == NFeTipoEvento.TeNfeCancelamento);

                if (evento == null)
                {
                    return;
                }

                _cupomFiscal.Cancelado(consulta.RetornoCompletoStr);
            }
            catch (WebException ex)
            {
                throw new InvalidOperationException($"Falha ao conectar com a SEFAZ: {ex.Message}");
            }
        }

        private void SolicitaCancelamento(ServicosNFe servico)
        {
            var cupomFinalizado = _cupomFiscal.CupomFiscalFinalizado;

            var evento = servico.RecepcaoEventoCancelamento(
                idlote: 1,
                sequenciaEvento: 1,
                protocoloAutorizacao: cupomFinalizado.Protocolo,
                chaveNFe: cupomFinalizado.Chave,
                justificativa: Justificativa.TrimSefaz().RemoverAcentos(),
                cpfcnpj: _venda.Empresa.DocumentoUnico
            );

            if (evento.Retorno.cStat != 128)
            {
                throw new InvalidOperationException($"Falha no lote - {evento.Retorno.xMotivo}");
            }

            var retEvento = evento.Retorno.retEvento?.FirstOrDefault(i => i.infEvento != null);

            if (retEvento == null)
            {
                throw new InvalidOperationException("Falha ao receber o evento de cancelamento da SEFAZ");
            }

            if (retEvento.infEvento.cStat != 135 && retEvento.infEvento.cStat != 136)
            {
                throw new InvalidOperationException(retEvento.infEvento.xMotivo);
            }

            _cupomFiscal.Cancelado(evento.RetornoCompletoStr);
        }

        private void SalvaOperacao(NfeInutilizacaoNumeracaoDTO inutilizacao)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var servico = new FaturamentoVendaService(sessao, SessaoSistema.Instancia.UsuarioLogado);

                if (_cupomFiscal != null)
                    new RepositorioCupomFiscal(sessao).SalvarOuAlterar(_cupomFiscal);

                if (inutilizacao != null)
                    new RepositorioInutilizacao(sessao).Salvar(inutilizacao);

                servico.Cancelar(_venda, Justificativa);
                transacao.Commit();
            }
        }

        private void CarregarCupomFiscal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                _cupomFiscal = repositorioCupomFiscal.ObterCupomFiscal(_venda);

                transacao.Commit();
            }
        }

        private void CarregarEmissorFiscal()
        {
            if (_cupomFiscal == null) return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _emissorFiscalNfce = new RepositorioEmissorFiscal(sessao)
                    .GetPeloId(_cupomFiscal.EmissorFiscalId).EmissorFiscalNfce;
            }
        }

        public virtual void OnCancelou()
        {
            CanceladoSucesso?.Invoke(this, EventArgs.Empty);
        }
    }
}