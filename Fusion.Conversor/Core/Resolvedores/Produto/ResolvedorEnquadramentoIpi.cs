using Fusion.Conversor.Core.Cache;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorEnquadramentoIpi : ResolvedorComum<EquadramentoIpi>
    {
        public ResolvedorEnquadramentoIpi(IStatelessSession session, ArrayCache<EquadramentoIpi> cache) 
            : base(session, cache)
        {
        }

        protected override EquadramentoIpi DoResolve(string input, EquadramentoIpi @default)
        {
            if (Cache.TryGetCache(i => i.Id == input, out var enquadramento))
            {
                return enquadramento;
            }

            enquadramento = Session.QueryOver<EquadramentoIpi>()
                .Where(i => i.Id == input)
                .SingleOrDefault();

            if (enquadramento == null)
            {
                return @default;
            }

            Cache.Add(enquadramento);

            return enquadramento;
        }
    }
}