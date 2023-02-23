using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Produtos
{
    public static class ProdutoAjuda
    {
        public static void ValidarUnidadeMedidaPodeFracionar(ProdutoDTO produto, decimal quantidade)
        {
            if (produto.ProdutoUnidadeDTO.PodeFracionar == 1) return;

            var quantidadeInteira = (int)quantidade;

            if (quantidadeInteira != quantidade)
                throw new InvalidOperationException("Unidade do produto não permite ser fracionada.");
        }
    }
}