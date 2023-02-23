using Fusion.Conversor.Core.Cache;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorCofins : ResolvedorComum<TributacaoCofins>
    {
        public ResolvedorCofins(IStatelessSession session, ArrayCache<TributacaoCofins> cache) 
            : base(session, cache)
        {
        }

        protected override TributacaoCofins DoResolve(string input, TributacaoCofins @default)
        {
            if (Cache.TryGetCache(i => i.Id == input, out var cofins))
            {
                return cofins;
            }

            cofins = Session.QueryOver<TributacaoCofins>()
                .Where(i => i.Id == input)
                .SingleOrDefault();

            if (cofins == null)
            {
                return @default;
            }

            Cache.Add(cofins);

            return cofins;
        }
    }
}