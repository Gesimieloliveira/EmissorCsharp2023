using System;

namespace FusionCore.FusionAdm.Produtos
{
    public class ProdutoHistoricoCompra
    {
        public int ItemId { get; set; }
        public int CompraId { get; set; }
        public int NumeroDocumento { get; set; }
        public int ProdutoId { get; set; }
        public string Fornecedor { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime ItemCadastradoEm { get; set; }
        public decimal Quantidade { get; set; }
        public string UnidadeCompra { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal DescontoTotal { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorTotalCusto { get; set; }
        public decimal FatorConversao { get; set; }
        public decimal QuantidadeConversao { get; set; }
    }
}