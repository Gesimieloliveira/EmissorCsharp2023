using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionCore.AutorizacaoOperacao.PayloadTypes
{
    public class NfeItemExcluido : IPayload
    {
        public NfeItemExcluido(
            int itemId,
            int nfeId,
            int produtoId,
            string produtoNome,
            decimal quantidade,
            decimal valorUnitario,
            decimal valorTotal)
        {
            ItemId = itemId;
            NfeId = nfeId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
        }

        public int ItemId { get; set; }
        public int NfeId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }

    }
}
