using FusionCore.Servicos.Core.Repositorios;
using NHibernate;

namespace FusionCore.Servicos.Core.Exportacao.Estrategias
{
    public class ExportacaoNfce : IExportacao
    {
        public IRepositorioExportacao CriaRepositorio(IStatelessSession session)
        {
            return new RepositorioNfce(session);
        }
    }
}