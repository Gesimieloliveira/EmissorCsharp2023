using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtContingenciaAdm
    {
        public static NfceContingenciaAdm ToAdm(this NfceContingencia contingencia)
        {
            var contingenciaAdm = new NfceContingenciaAdm
            {
                Ativa = contingencia.Ativa,
                Motivo = contingencia.Motivo,
                EntrouEm = contingencia.EntrouEm,
                Id = 0
            };

            return contingenciaAdm;
        }
    }
}