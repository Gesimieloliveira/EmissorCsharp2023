namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsTributacao
    {
        public int CteOsId { get; set; }
        public CteOs CteOs { get; set; }
        public decimal ValorIbpt { get; set; }
        public decimal Inss { get; set; }
    }
}