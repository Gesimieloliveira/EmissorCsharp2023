namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsConfigImposto
    {
        public int CteOsId { get; set; }
        public CteOs CteOs { get; set; }
        public bool IsCalculosAutomaticos { get; set; }
        public bool IsPartilha { get; set; }
        public bool IsCreditoIcmsAutomatico { get; set; }
        public bool UsarTributacaoFederal { get; set; }
    }
}