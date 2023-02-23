using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscalMDFE
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.MDFe;
        public short Serie { get; set; }
        public int NumeroAtual { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
    }
}