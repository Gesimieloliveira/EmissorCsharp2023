using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.Exportacao.ItensBuscaRapida
{
    public class RepositorioBuscaRapida
    {
        private readonly IStatelessSession _session;
        private readonly Linha _item = null;

        public RepositorioBuscaRapida(IStatelessSession session)
        {
            _session = session;
        }

        public IEnumerable<Linha> BuscaTodos()
        {
            ProdutoDTO produtoAlias = null;
            ProdutoAlias produtoCodigoBarrasAlias = null;

            var eUmCodigoBarras =
                Restrictions.Eq(Projections.Property(() => produtoCodigoBarrasAlias.IsCodigoBarras), true);

            var subquery = QueryOver.Of(() => produtoCodigoBarrasAlias)
                .Select(alias => alias.Alias)
                .Where(eUmCodigoBarras)
                .Where(() => produtoCodigoBarrasAlias.Produto.Id == produtoAlias.Id)
                .OrderByAlias(() => produtoCodigoBarrasAlias.Id).Desc.Take(1);

            var query = _session.QueryOver(() => produtoAlias)
                .JoinAlias(() => produtoAlias.ProdutosAlias, () => produtoCodigoBarrasAlias)
                .SelectList(list => list
                    .SelectSubQuery(subquery).WithAlias(() => _item.CodigoBarras)
                    .Select(() => produtoAlias.Nome).WithAlias(() => _item.Nome)
                    .Select(() => produtoAlias.PrecoVenda).WithAlias(() => _item.Preco)
                );

            query.Where(eUmCodigoBarras);

            query.TransformUsing(Transformers.AliasToBean<Linha>());

            return query.List<Linha>();
        }
    }
}