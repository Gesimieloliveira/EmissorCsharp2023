using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsSeguro : Entidade
    {
        public int Id { get; set; }
        public CteOs CteOs { get; set; }
        public ResponsavelSeguro ResponsavelSeguro { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }

        protected override int ReferenciaUnica => Id;
    }
}