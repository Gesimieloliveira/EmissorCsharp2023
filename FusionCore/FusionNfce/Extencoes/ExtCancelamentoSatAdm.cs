using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionNfce.Fiscal.SatFiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtCancelamentoSatAdm
    {
        public static CancelamentoSatAdm ToAdm(this CancelamentoSat cancelamentoSat, NfceAdm nfceAdm)
        {
            if (cancelamentoSat == null) return null;

            var cancelamentoSatAdm = new CancelamentoSatAdm
            {
                Nfce = nfceAdm,
                XmlRetorno = cancelamentoSat.XmlRetorno,
                NfceId = 0,
                AmbienteSefaz = cancelamentoSat.AmbienteSefaz,
                EnviadoEm = cancelamentoSat.EnviadoEm,
                XmlEnvio = cancelamentoSat.XmlEnvio,
                CodigoRetorno = cancelamentoSat.CodigoRetorno
            };

            return cancelamentoSatAdm;
        }
    }
}