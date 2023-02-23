using Fusion.Conversor.Core.Cache;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorPis : ResolvedorComum<TributacaoPis>
    {
        public ResolvedorPis(IStatelessSession session, ArrayCache<TributacaoPis> cache) 
            : base(session, cache)
        {
        }

        protected override TributacaoPis DoResolve(string input, TributacaoPis @default)
        {
            if (Cache.TryGetCache(i => i.Id == input, out var pis))
            {
                return pis;
            }

            pis = Session.QueryOver<TributacaoPis>()
                .Where(i => i.Id == input)
                .SingleOrDefault();

            if (pis == null)
            {
                return @default;
            }

            Cache.Add(pis);

            return pis;
        }
    }
}