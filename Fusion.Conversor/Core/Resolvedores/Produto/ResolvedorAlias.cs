using System;
using Fusion.Conversor.Core.Cache;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorAlias : ResolvedorCacheable<ProdutoAlias>
    {
        public ResolvedorAlias(IStatelessSession session, ArrayCache<ProdutoAlias> cache) : base(session, cache)
        {
        }

        public ProdutoAlias Resolve(ProdutoDTO produto, string alias)
        {
            if (Cache.TryGetCache(i => i.Alias == alias, out var finded))
            {
                throw new InvalidOperationException(
                    $"Já foi inserido um código de barras igual a este: {alias}. Foi vinculo para o item: {finded.Produto}");
            }

            var novoAlias = ProdutoAlias.Criar(produto, alias);

            Cache.Add(novoAlias);

            return novoAlias;
        }
    }
}