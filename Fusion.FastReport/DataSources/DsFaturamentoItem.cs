namespace Fusion.FastReport.DataSources
{
    public struct DsFaturamentoItem
    {
        public int ItemId { get; set; }
        public int ProdutoId { get; set; }
        public int FaturamentoId { get; set; }
        public string Descricao { get; set; }
        public string SiglaUnidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Total { get; set; }
    }
}