using FusionCore.Servicos.Core.Repositorios;
using NHibernate;

namespace FusionCore.Servicos.Core.Exportacao.Estrategias
{
    public class ExportacaoNfe : IExportacao
    {
        public IRepositorioExportacao CriaRepositorio(IStatelessSession session)
        {
            return new RepositorioNfe(session);
        }
    }
}