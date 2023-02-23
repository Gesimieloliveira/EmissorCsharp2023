using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace Fusion.Conversor.Core.Repositorios.CustomQueries
{
    public static class TabelaProduto
    {
        public static int UltimoCodigo(IStatelessSession session)
        {
            var q = session.QueryOver<ProdutoDTO>()
                .Select(i => i.Id)
                .OrderBy(i => i.Id).Desc
                .Take(1);

            return q.SingleOrDefault<int>();
        }
    }
}