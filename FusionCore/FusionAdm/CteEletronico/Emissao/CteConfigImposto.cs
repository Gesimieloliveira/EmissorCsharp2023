namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteConfigImposto
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public bool IsCalculosAutomaticos { get; set; }
        public bool IsPartilha { get; set; }
        public bool IsCreditoIcmsAutomatico { get; set; }
    }
}