using FusionCore.FusionAdm.Emissores.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.EmissorFiscal
{
    public class NfceEmissorFiscalSat : Entidade
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; } = ModeloDocumento.SAT;
        public short NumeroCaixa { get; set; }
        public CodificacaoArquivoXml CodificacaoArquivoXml { get; set; } = CodificacaoArquivoXml.Windows1252;
        public string CodigoAtivacao { get; set; }
        public string CodigoAcossiacao { get; set; }
        public Modelo Fabricante { get; set; }
        public NfceEmissorFiscal EmissorFiscal { get; set; }
        public VersaoSat VersaoLayoutSat { get; set; }
        public bool IsMFe { get; set; }
        public string ChaveAcessoValidador { get; set; }

        protected override int ReferenciaUnica => EmissorFiscalId;
    }
}