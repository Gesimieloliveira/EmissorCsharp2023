using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtEmitenteAdm
    {
        public static NfceEmitenteAdm ToAdm(this NfceEmitente emitente, NfceAdm nfceAdm, EmpresaDTO empresa)
        {
            if (emitente == null) return null;

            var emissaoAdm = new NfceEmitenteAdm
            {
                Nfce = nfceAdm,
                NfceId = 0,
                Empresa = empresa
            };

            return emissaoAdm;
        }
    }
}