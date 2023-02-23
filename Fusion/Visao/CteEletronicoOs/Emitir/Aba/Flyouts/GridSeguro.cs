using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts
{
    public class GridSeguro
    {
        public ResponsavelSeguro ResponsavelSeguro { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public CteOsSeguro CteOsSeguro { get; set; }
    }
}