using FusionCore.Tributacoes.Estadual;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteImpostoCst
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public decimal PercentualCredito { get; set; }
        public decimal ValorCredito { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal BaseCalculoIcmsSt { get; set; }
        public decimal PercentualIcms { get; set; }
        public decimal PercentualIcmsSt { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal ValorIcmsSt { get; set; }
        public decimal PercentualReducaoBc { get; set; }
        public TributacaoIcms TributacaoIcms  { get; set; }
    }
}