namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteVeiculoTransportado
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public string Cor { get; set; }
        public string DescricaoCor { get; set; }
        public string CodigoMarcaModelo { get; set; }
        public string Chassi { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal FreteUnitario { get; set; } 
    }
}