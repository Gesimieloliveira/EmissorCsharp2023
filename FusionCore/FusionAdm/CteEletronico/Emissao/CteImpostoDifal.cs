namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteImpostoDifal
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal PercentualFcp { get; set; }
        public decimal PercentualAliquotaInterna { get; set; }
        public decimal PercentualAliquotaInterestadual { get; set; }
        public decimal PercentualProvisorio { get; set; }
        public decimal ValorIcmsFcp { get; set; }
        public decimal ValorIcmsUfTermino { get; set; }
        public decimal ValorIcmsUfInicio { get; set; }
        public string Observacao { get; set; }
    }
}