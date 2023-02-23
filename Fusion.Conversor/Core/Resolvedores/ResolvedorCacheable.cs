using Fusion.Conversor.Core.Cache;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores
{
    public abstract class ResolvedorCacheable<T> : Resolvedor
    {
        protected readonly ArrayCache<T> Cache;

        protected ResolvedorCacheable(IStatelessSession session, ArrayCache<T> cache) : base(session)
        {
            Cache = cache;
        }
    }
}