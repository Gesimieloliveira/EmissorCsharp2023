using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsTributacaoFederal : Entidade
    {
        public int CteOsId { get; set; }
        protected override int ReferenciaUnica => CteOsId;
        public CteOs CteOs { get; set; }
        public decimal ValorPis { get; set; }
        public decimal ValorCofins { get; set; }
        public decimal ValorImpostoRenda { get; set; }
        public decimal ValorInss { get; set; }
        public decimal ValorClss { get; set; }
    }
}