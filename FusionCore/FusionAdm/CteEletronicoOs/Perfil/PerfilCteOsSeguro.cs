using FusionCore.FusionAdm.CteEletronicoOs.Flags;

namespace FusionCore.FusionAdm.CteEletronicoOs.Perfil
{
    public class PerfilCteOsSeguro
    {
        public int PerfilCteOsId { get; set; }
        public PerfilCteOs Perfil { get; set; }
        public ResponsavelSeguro ResponsavelSeguro { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
    }
}