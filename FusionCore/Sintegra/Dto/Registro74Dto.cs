namespace FusionCore.Sintegra.Dto
{
    public class Registro74Dto : IRegistro74Dto
    {
        public int CodigoProdutoServico { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public string GetCodigoProdutoServico()
        {
            return CodigoProdutoServico.ToString();
        }

        public decimal GetQuantidade()
        {
            return Quantidade;
        }

        public decimal GetValorBurto()
        {
            return Quantidade * ValorUnitario;
        }
    }
}