using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Componentes;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionAdm.Fiscal.NF.CCe
{
    public class SolicitacaoCCe
    {
        public DateTime SolicitadoEm { get; }
        public string Correcao { get; }
        public string ChaveNfe => Nfe.NumeroChave;
        public short UfEmissaoNfe => Nfe.Emitente.Empresa.EstadoDTO.CodigoIbge;
        public TipoAmbiente AmbienteNfe => Nfe.Finalizacao.TipoAmbiente;
        public DocumentoUnico DocumentoUnico => new DocumentoUnico(Nfe.Emitente.DocumentoUnicoSemZeroAEsquerda);
        public bool NfeFoiEmitida => Nfe.TemEmissao;
        public Nfeletronica Nfe { get; }

        public SolicitacaoCCe(Nfeletronica nfe, string correcao)
        {
            Nfe = nfe;
            Correcao = correcao.RemoverAcentos();
            SolicitadoEm = DateTime.Now;
        }

        public CartaCorrecaoNfe CriaCce()
        {
            return new CartaCorrecaoNfe(Nfe, Correcao)
            {
                OcorreuEm = SolicitadoEm
            };
        }
    }
}