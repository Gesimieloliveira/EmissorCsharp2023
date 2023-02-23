using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Nfce.SatFiscal
{
    public class CancelamentoSatAdm
    {
        public int NfceId { get; set; }
        public NfceAdm Nfce { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public DateTime EnviadoEm { get; set; }
        public int CodigoRetorno { get; set; }
    }
}