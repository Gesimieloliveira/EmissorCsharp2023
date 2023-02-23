using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.FusionNfce.Empresa;

namespace FusionCore.FusionNfce.Fiscal.SatFiscal
{
    public class FinalizaEmissaoSat
    {
        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public EmpresaNfce Empresa { get; set; }
        public short NumeroCaixa { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public string XmlRetorno { get; set; }
        public string Chave { get; set; }
        public int NumeroDocumento { get; set; }
        public int CodigoErro { get; set; }
        public int CodigoRetorno { get; set; }
        public int CodigoSefaz { get; set; }
        public string MensagemRetorno { get; set; }
        public string MensagemSefaz { get; set; }
        public string NumeroSessao { get; set; }
        public string QrCode { get; set; }
        public NfceEmissorFiscal EmissorFiscal { get; set; }
    }
}