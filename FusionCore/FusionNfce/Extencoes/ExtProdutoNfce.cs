using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtProdutoNfce
    {
        public static ProdutoDTO ToAdm(this ProdutoNfce produto)
        {
            var produtoDto = new ProdutoDTO
            {
                Id = produto.Id
            };

            return produtoDto;
        }
    }
}