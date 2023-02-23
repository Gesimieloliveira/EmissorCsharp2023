using NHibernate;

namespace FusionCore.Servicos.Core.Exportacao.Estrategias
{
    public interface IExportacao
    {
        IRepositorioExportacao CriaRepositorio(IStatelessSession session);
    }
}