namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsComponenteValorPrestacao
    {
        public int Id { get; set; }
        public CteOs CteOs { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}