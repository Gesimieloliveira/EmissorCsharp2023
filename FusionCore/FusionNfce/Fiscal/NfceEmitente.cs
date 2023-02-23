using FusionCore.FusionNfce.Empresa;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceEmitente : Entidade
    {
        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public EmpresaNfce Empresa { get; set; }
        protected override int ReferenciaUnica => NfceId;
    }
}