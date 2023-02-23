using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using JetBrains.Annotations;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NHibernate;

namespace Fusion.Visao.NotaFiscalEletronica.Cancelamento
{
    public class NfeCancelamentoContexto : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private IList<DocumentoReceber> _documentoVinculados;
        private readonly EmissorFiscalNFE _emissor;
        private readonly UsuarioDTO _usuario;
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();

        public NfeCancelamentoContexto(Nfeletronica nfe, EmissorFiscalNFE emissor, UsuarioDTO usuario)
        {
            _nfe = nfe;
            _emissor = emissor;
            _usuario = usuario;
            _documentoVinculados = new List<DocumentoReceber>();
        }

        public string ChaveDocumento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int NumeroDocumento
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Justificativa precisa ter no mínimo 15 caracteres")]
        [MinLength(15, ErrorMessage = "Justificativa precisa ter no mínimo 15 caracteres")]
        public string Justificativa
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public void Inicializa()
        {
            SetValue(_nfe.NumeroChave, nameof(ChaveDocumento));
            SetValue(_nfe.NumeroDocumento, nameof(NumeroDocumento));
        }

        public void FazCancelamento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuario);

            ThrowExceptionSeExistirErros();
            CarregaDocumentosReceber();
            ThrowExceptionSeExisteAlgumDocumentoComQuitacao();

            var cfgBuilder = new ConfiguracaoZeusBuilder(_emissor, _nfe.Finalizacao.TipoEmissao);
            var configuracao = cfgBuilder.GetConfiguracao();

            using (var servico = new ServicosNFe(configuracao))
            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                EstornaDocumentosVinculados(sessao);
                EstornarCaixa(sessao);
                EstornaEstoque(sessao);

                var cancelamento = VerificaCancelamentoSefaz(servico);

                if (cancelamento == null)
                {
                    cancelamento = SolicitaEventoCancelamento(servico);
                }

                if (cancelamento.Status.EstaCancelado == false)
                {
                    throw new InvalidOperationException(cancelamento.TextoResposta);
                }

                _nfe.Cancelar(cancelamento);

                new RepositorioNfe(sessao).SalvarAlteracoes(_nfe);

                sessao.Transaction.Commit();
            }
        }

        private void CarregaDocumentosReceber()
        {
            if (_nfe.Malote == null)
            {
                _documentoVinculados = new List<DocumentoReceber>();
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioDocumentoReceber(sessao);
                _documentoVinculados = repositorio.BuscarPeloMaloteId(_nfe.Malote.Id);
            }
        }

        private void ThrowExceptionSeExisteAlgumDocumentoComQuitacao()
        {
            if (_documentoVinculados.Any(i => i.ValorQuitado != 0))
            {
                throw new InvalidOperationException("Existem documentos a receber com quitação para essa NF-e. Preciso que estorne-os.");
            }
        }

        private void EstornaDocumentosVinculados(ISession sessao)
        {
            if (!_documentoVinculados.Any())
            {
                return;
            }

            var servicoCaixa = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);
            var servicoDocumento = new ServicoDocumentoReceber(sessao, servicoCaixa);

            foreach (var doc in _documentoVinculados)
            {
                servicoDocumento.FazerCancelamento(doc, _usuario);
            }
        }

        private void EstornarCaixa(ISession sessao)
        {
            var servico = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);

            servico.RegistrarEstorno(_nfe, _usuario);
        }

        private void EstornaEstoque(ISession sessao)
        {
            NfeEstoqueHelper.CancelamentoNfe(_nfe, _usuario, sessao);
        }

        [CanBeNull]
        private CancelamentoNfe VerificaCancelamentoSefaz(ServicosNFe servico)
        {
            try
            {
                var consulta = servico.NfeConsultaProtocolo(_nfe.NumeroChave);

                var evento = consulta.Retorno.procEventoNFe
                    .SingleOrDefault(i => i.evento.infEvento.chNFe == _nfe.NumeroChave && i.evento.infEvento.tpEvento == NFeTipoEvento.TeNfeCancelamento);

                if (evento == null)
                {
                    return null;
                }

                var cancelamento = new CancelamentoNfe(
                    _nfe,
                    _emissor.Ambiente,
                    consulta.Retorno.cStat,
                    consulta.Retorno.xMotivo,
                    new StatusCancelamento(evento.retEvento.infEvento.cStat, evento.retEvento.infEvento.xMotivo), 
                    Justificativa,
                    consulta.EnvioStr,
                    consulta.RetornoCompletoStr,
                    evento.retEvento.infEvento.dhRegEvento
                );

                return cancelamento;
            }
            catch (WebException ex)
            {
                throw new InvalidOperationException($"Falha ao conectar com a SEFAZ: {ex.Message}");
            }
        }

        private CancelamentoNfe SolicitaEventoCancelamento(ServicosNFe servico)
        {
            var evento = servico.RecepcaoEventoCancelamento(
                idlote: 1,
                sequenciaEvento: 1,
                protocoloAutorizacao: _nfe.NumeroProtocolo,
                chaveNFe: _nfe.NumeroChave,
                justificativa: Justificativa.TrimSefaz().RemoverAcentos(),
                cpfcnpj: _nfe.DocumentoUnico
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

            if (retEvento.infEvento.cStat != 135 
                && retEvento.infEvento.cStat != 136
                && retEvento.infEvento.cStat != 150
                && retEvento.infEvento.cStat != 151
                && retEvento.infEvento.cStat != 155)
            {
                throw new InvalidOperationException(retEvento.infEvento.xMotivo);
            }

            var cancelamento = new CancelamentoNfe(
                _nfe,
                _emissor.Ambiente,
                evento.Retorno.cStat,
                evento.Retorno.xMotivo,
                new StatusCancelamento(retEvento.infEvento.cStat, retEvento.infEvento.xMotivo),
                Justificativa,
                evento.EnvioStr,
                evento.RetornoCompletoStr,
                retEvento.infEvento.dhRegEvento
            );

            return cancelamento;
        }
    }
}