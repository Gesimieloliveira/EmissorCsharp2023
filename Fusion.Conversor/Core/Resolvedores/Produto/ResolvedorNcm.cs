using Fusion.Conversor.Core.Cache;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorNcm : ResolvedorComum<NcmDTO>
    {
        public ResolvedorNcm(IStatelessSession session, ArrayCache<NcmDTO> cache) 
            : base(session, cache)
        {
        }

        protected override NcmDTO DoResolve(string input, NcmDTO @default)
        {
            input = StringPreparer.RemoveNaoNumeros(input);

            if (Cache.TryGetCache(i => i.Id == input, out var ncm))
            {
                return ncm;
            }

            ncm = Session.QueryOver<NcmDTO>()
                .Where(i => i.Id == input)
                .SingleOrDefault();

            if (ncm != null)
            {
                Cache.Add(ncm);
                return ncm;
            }

            return @default;
        }
    }
}