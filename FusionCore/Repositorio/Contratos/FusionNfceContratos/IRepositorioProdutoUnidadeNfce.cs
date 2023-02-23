using FusionCore.FusionNfce.Produto;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioProdutoUnidadeNfce : IRepositorio<ProdutoUnidadeNfce, int>
    {
        void Salvar(ProdutoUnidadeNfce produtoUnidade);
    }
}