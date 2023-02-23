namespace FusionCore.FusionAdm.PedidoVenda
{
    public struct ItemValue
    {
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal PorcentagemDesconto { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal TotalLiquido { get; set; }
        public string Observacao { get; set; }
        public decimal QuantidadeAntiga { get; set; }
    }
}