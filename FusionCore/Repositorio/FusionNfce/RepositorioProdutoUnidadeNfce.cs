using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioProdutoUnidadeNfce : Repositorio<ProdutoUnidadeNfce, int>, IRepositorioProdutoUnidadeNfce
    {
        public RepositorioProdutoUnidadeNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ProdutoUnidadeNfce produtoUnidade)
        {
            Sessao.SaveOrUpdate(produtoUnidade);
        }
    }
}