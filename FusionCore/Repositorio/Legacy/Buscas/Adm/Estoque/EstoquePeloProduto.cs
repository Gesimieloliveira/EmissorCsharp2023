using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Estoque
{
    public class EstoquePeloProduto : IBuscaUnico<ProdutoEstoqueDTO>
    {
        private readonly int _produtoId;

        public EstoquePeloProduto(int produtoId)
        {
            _produtoId = produtoId;
        }

        public EstoquePeloProduto(ProdutoDTO produto)
            : this(produto.Id)
        {
        }

        public ProdutoEstoqueDTO Busca(ISession sessao)
        {
            return sessao.Get<ProdutoEstoqueDTO>(_produtoId);
        }
    }
}