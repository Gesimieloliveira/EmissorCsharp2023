using Fusion.Conversor.Core.Cache;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorIpi : ResolvedorComum<TributacaoIpi>
    {
        public ResolvedorIpi(IStatelessSession session, ArrayCache<TributacaoIpi> cache) 
            : base(session, cache)
        {
        }

        protected override TributacaoIpi DoResolve(string input, TributacaoIpi @default)
        {
            if (Cache.TryGetCache(i => i.Codigo == input, out var ipi))
            {
                return ipi;
            }

            ipi = Session.QueryOver<TributacaoIpi>()
                .Where(i => i.Codigo == input)
                .SingleOrDefault();

            if (ipi == null)
            {
                return @default;
            }

            Cache.Add(ipi);

            return ipi;
        }
    }
}