using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionCore.AutorizacaoOperacao.PayloadTypes
{
    public class FaturamentoItemExcluido : IPayload
    {
        public FaturamentoItemExcluido(
            int itemId,
            int faturamentoId,
            int produtoId,
            string produtoNome,
            decimal quantidade,
            decimal valorUnitario,
            decimal valorTotal)
        {
            ItemId = itemId;
            FaturamentoId = faturamentoId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
        }

        public int ItemId { get; set; }
        public int FaturamentoId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }

    }
}
