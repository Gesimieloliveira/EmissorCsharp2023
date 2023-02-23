using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtNfceAdm
    {
        public static NfceAdm ToAdm(this Nfce nfce)
        {
            return NfceAdm.Cria(nfce);
        }
    }
}