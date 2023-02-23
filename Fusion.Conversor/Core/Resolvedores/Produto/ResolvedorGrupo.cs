using Fusion.Conversor.Core.Cache;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorGrupo : ResolvedorComum<ProdutoGrupoDTO>
    {
        public ResolvedorGrupo(IStatelessSession session, ArrayCache<ProdutoGrupoDTO> cache) 
            : base(session, cache)
        {
        }

        protected override ProdutoGrupoDTO DoResolve(string input, ProdutoGrupoDTO @default)
        {
            if (input.Length < 2)
            {
                return @default;
            }

            if (Cache.TryGetCache(i => i.Nome == input, out var grupo))
            {
                return grupo;
            }

            grupo = Session.QueryOver<ProdutoGrupoDTO>()
                .Where(i => i.Nome == input)
                .Take(1)
                .SingleOrDefault();

            if (grupo != null)
            {
                Cache.Add(grupo);
                return grupo;
            }

            grupo = new ProdutoGrupoDTO {Nome = input };

            Session.Insert(grupo);
            Cache.Add(grupo);

            return grupo;
        }
    }
}