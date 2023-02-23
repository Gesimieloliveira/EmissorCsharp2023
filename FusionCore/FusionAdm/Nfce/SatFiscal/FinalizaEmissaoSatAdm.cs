using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Nfce.SatFiscal
{
    public class FinalizaEmissaoSatAdm 
    {
        public int NfceId { get; set; }
        public NfceAdm Nfce { get; set; }
        public EmpresaDTO Empresa { get; set; }
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
        public EmissorFiscal EmissorFiscal { get; set; }
    }
}