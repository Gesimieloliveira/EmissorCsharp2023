using FusionCore.Tributacoes.Estadual;

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsImpostoCst
    {
        public int CteOsId { get; set; }
        public CteOs CteOs { get; set; }
        public TributacaoIcms TributacaoIcms { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Aliquota { get; set; }
        public decimal Valor { get; set; }
        public decimal PercentualReducao { get; set; }
        public decimal PercentualCredito { get; set; }
        public decimal ValorCredito { get; set; }
    }
}