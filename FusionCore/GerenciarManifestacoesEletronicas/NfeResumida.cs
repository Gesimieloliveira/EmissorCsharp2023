using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class NfeResumida : EntidadeBase<int>
    {
        public int Id { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public string Chave { get; set; }
        public short Serie { get; set; }
        public string NumeroFiscal { get; set; }
        public string DocumentoUnicoEmitente { get; set; }
        public string InscricaoEstadualEmitente { get; set; }
        public DateTime EmitidaEm { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public decimal Valor { get; set; }
        public DateTime AutorizacaoEm { get; set; }
        public string NumeroProtocolo { get; set; }
        public string RazaoSocialEmitente { get; set; }
        public StatusNfe StatusNfe { get; set; }
        public StatusManifestacao StatusManifestacao { get; set; } = StatusManifestacao.Desconhecida;
        public string Xml { get; set; }
        public IList<CartaCorrecaoDFe> CartaCorrecaoLista { get; set; } = new List<CartaCorrecaoDFe>();
        public IList<EventoManifestacao> EventoManifestacaoLista { get; set; } = new List<EventoManifestacao>();
        public EmissorFiscal EmissorFiscal { get; set; }
        public bool IsImportada { get; set; }

        public CancelamentoDFe Cancelamento { get; private set; }
        public DownloadXmlDFe DownloadXml { get; private set; }

        public void AddXmlDownload(string xml)
        {
            if (DownloadXml == null)
            {
                DownloadXml = new DownloadXmlDFe
                {
                    NfeResumida = this
                };
            }

            DownloadXml.Xml = xml;
        }

        public void AddCancelamento(string xml)
        {
            if (Cancelamento == null)
            {
                Cancelamento = new CancelamentoDFe
                {
                    NfeResumida = this
                };
            }

            Cancelamento.Xml = xml;
        }

        public void CienciaEmissao()
        {
            if (StatusManifestacao != StatusManifestacao.DownloadXml)
                StatusManifestacao = StatusManifestacao.CienciaOperacao;
        }

        public void ConfirmacaoOperacao()
        {
            if (StatusManifestacao != StatusManifestacao.DownloadXml)
                StatusManifestacao = StatusManifestacao.ConfirmacaoOperacao;
        }

        public void DesconhecimentoOperacao()
        {
            if (StatusManifestacao != StatusManifestacao.DownloadXml)
                StatusManifestacao = StatusManifestacao.DesconhecimentoOperacao;
        }

        public void OperacaoNaoRealizada()
        {
            if (StatusManifestacao != StatusManifestacao.DownloadXml)
                StatusManifestacao = StatusManifestacao.OperacaoNaoRealizada;
        }

        public void DownloadXmlStatus()
        {
            StatusManifestacao = StatusManifestacao.DownloadXml;
        }

        protected override int ChaveUnica => Id;

        public bool IsContemCienciaEmissaoEvento()
        {
            return EventoManifestacaoLista.Any(x => x.Evento == StatusManifestacao.CienciaOperacao);
        }

        public bool IsContemConfirmacaoOperacaoEvento()
        {
            return EventoManifestacaoLista.Any(x => x.Evento == StatusManifestacao.ConfirmacaoOperacao);
        }

        public bool IsContemOperacaoNaoRealizadaEvento()
        {
            return EventoManifestacaoLista.Any(x => x.Evento == StatusManifestacao.OperacaoNaoRealizada);
        }

        public bool IsContemDesconhecimentoDaOperacaoEvento()
        {
            return EventoManifestacaoLista.Any(x => x.Evento == StatusManifestacao.DesconhecimentoOperacao);
        }

        public bool IsDownloadDisponivel()
        {
            return StatusManifestacao == StatusManifestacao.DownloadXml;
        }

        public bool IsPodeEfetuarDownload()
        {
            return IsContemCienciaEmissaoEvento()
                   || IsContemConfirmacaoOperacaoEvento()
                   || IsContemOperacaoNaoRealizadaEvento()
                   || IsContemDesconhecimentoDaOperacaoEvento();
        }

        public string DownloadXmlString()
        {
            return DownloadXml.Xml;
        }

        public void ImportacaoConcluida()
        {
            IsImportada = true;
        }

        public void DeletarImportacao()
        {
            IsImportada = false;
        }
    }
}