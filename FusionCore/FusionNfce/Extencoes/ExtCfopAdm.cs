using FusionCore.FusionNfce.Cfop;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtCfopAdm
    {
        public static CfopDTO ToAdm(this CfopNfce cfopNfce)
        {
            var cfop = new CfopDTO
            {
                Descricao = cfopNfce.Descricao,
                Id = cfopNfce.Id,
                EntidadeSincronizavel = EntidadeSincronizavel.NaoSincronizar
            };

            return cfop;
        }
    }
}