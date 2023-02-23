using System;

namespace FusionCore.FusionAdm.Compras
{
    public class NotaFiscalCompraGrid
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Serie { get; set; }
        public decimal TotalItens { get; set; }
        public decimal ValorTotal { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeEmpresa { get; set; }

        public DateTime EmissaoEm { get; set; }
        public DateTime EntradaEm { get; set; }
        public string Chave { get; set; }
        public decimal TotalBcIcms { get; set; }
        public decimal ValorTotalIcms { get; set; }
        public decimal TotalBcIcmsSt { get; set; }
        public decimal ValorTotalIcmsSt { get; set; }
        public decimal ValorTotalIpi { get; set; }
        public decimal ValorTotalFrete { get; set; }
        public decimal ValorTotalSeguro { get; set; }
        public decimal ValorTotalDesconto { get; set; }
        public decimal ValorTotalOutros { get; set; }
    }
}
