using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtHistoricoSatAdm
    {
        public static HistoricoEnvioSatAdm ToAdm(this HistoricoEnvioSat historicoEnvioSat, Nfce nfce)
        {
            var historico = new HistoricoEnvioSatAdm
            {
                Nfce = nfce.ToAdm(),
                Id = 0,
                AmbienteSefaz = historicoEnvioSat.AmbienteSefaz,
                XmlEnvio = historicoEnvioSat.XmlEnvio,
                MensagemRetorno = historicoEnvioSat.MensagemRetorno.TrimOrEmpty(),
                CodigoErro = historicoEnvioSat.CodigoErro,
                NumeroCaixa = historicoEnvioSat.NumeroCaixa,
                CodigoSefaz = historicoEnvioSat.CodigoSefaz,
                CodigoRetorno = historicoEnvioSat.CodigoRetorno,
                Empresa = historicoEnvioSat.Empresa.ToAdm(),
                MensagemSefaz = historicoEnvioSat.MensagemSefaz.TrimOrEmpty(),
                NumeroSessao = historicoEnvioSat.NumeroSessao.TrimOrEmpty(),
                Finalizou = historicoEnvioSat.Finalizou
            };

            return historico;
        }
    }
}