using FusionCore.FusionAdm.Emissores.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscalSAT : ISincronizavelAdm
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; } = ModeloDocumento.SAT;
        public short NumeroCaixa { get; set; }
        public CodificacaoArquivoXml CodificacaoArquivoXml { get; set; } = CodificacaoArquivoXml.UTF8;
        public VersaoSat VersaoLayoutSat { get; set; } = VersaoSat.V7;
        public string CodigoAtivacao { get; set; }
        public string CodigoAcossiacao { get; set; }
        public Modelo Fabricante { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public bool IsMFe { get; set; }
        public string ChaveAcessoValidador { get; set; }

        public string Referencia => EmissorFiscalId.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.EmissorFiscal;
    }
}