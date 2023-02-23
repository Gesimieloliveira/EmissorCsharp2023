using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionNfce.Fiscal.SatFiscal
{
    public class CancelamentoSat
    {
        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public int CodigoRetorno { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public DateTime EnviadoEm { get; set; }
    }
}