namespace Fusion.Ibpt.Contexto.Modelo
{
    public class Produto
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public decimal TributoFederal { get; set; }
        public decimal TributoEstadual { get; set; }
    }
}