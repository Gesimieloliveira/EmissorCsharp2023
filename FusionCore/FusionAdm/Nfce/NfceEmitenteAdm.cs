using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Nfce
{
    public class NfceEmitenteAdm
    {
        public int NfceId { get; set; }
        public NfceAdm Nfce { get; set; }
        public EmpresaDTO Empresa { get; set; }
    }
}