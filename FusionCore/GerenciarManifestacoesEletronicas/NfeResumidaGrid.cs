using System;
using DFe.Ext;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class NfeResumidaGrid
    {

        public decimal ValorNFe { get; set; }

        public DateTime AutorizacaoEm { get; set; }

        public StatusManifestacao SituacaoManifestacao { get; set; }

        public string Chave { get; set; }

        public string RazaoSocialEmitente { get; set; }

        public string NumeracaoFiscal { get; set; }

        public string SituacaoInformativa => StatusAtual.Descricao();

        public StatusNfe StatusAtual { get; set; }

        public TipoOperacao TipoOperacao { get; set; }

        public TipoAmbiente Ambiente { get; set; }

        public bool IsImportada { get; set; }

        public string ImportadaTexto => GetDescricaoIsImportada();

        private string GetDescricaoIsImportada()
        {
            return IsImportada ? "Importação Concluida" : "Importação Pendente";
        }

        public bool IsFinalizada => StatusAtual != StatusNfe.Pendente;
        public bool IsAutorizada => StatusAtual == StatusNfe.Autorizada;
        public bool IsDenegada => StatusAtual == StatusNfe.Denegada;
        public bool IsCancelada => StatusAtual == StatusNfe.Cancelada;
        public bool IsPendente => StatusAtual == StatusNfe.Pendente;
        public bool IsDownloadXml => SituacaoManifestacao == StatusManifestacao.DownloadXml;
        public bool IsCienciaEmissao => SituacaoManifestacao == StatusManifestacao.CienciaOperacao;
        public int NfeResumidaId { get; set; }

        public NfeResumida GetNfeResumida()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioDistribuicaoDFe(sessao).NfeResumidaPelo(NfeResumidaId);
            }
        }
    }
}