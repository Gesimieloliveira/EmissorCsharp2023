using NHibernate;

namespace FusionCore.Repositorio.Filtros
{
    public interface IFiltro
    {
        void Aplicar<TRoot, TSub>(IQueryOver<TRoot, TSub> queryover);
    }
}