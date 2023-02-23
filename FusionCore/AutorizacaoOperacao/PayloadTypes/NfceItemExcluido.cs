using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionCore.AutorizacaoOperacao.PayloadTypes
{
    public class NfceItemExcluido : IPayload
    {
        public NfceItemExcluido(
            int itemId,
            int nfceId,
            int produtoId,
            string produtoNome,
            decimal quantidade,
            decimal valorUnitario,
            decimal valorTotal)
        {
            ItemId = itemId;
            NfceId = nfceId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
        }

        public int ItemId { get; set; }
        public int NfceId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }

    }
}
