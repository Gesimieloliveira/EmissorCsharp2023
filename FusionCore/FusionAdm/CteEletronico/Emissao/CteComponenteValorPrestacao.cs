namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteComponenteValorPrestacao
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}