using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioConfiguracaoEmail : Repositorio<ConfiguracaoEmailDTO, int>, IRepositorioConfiguracaoEmail
    {
        public RepositorioConfiguracaoEmail(ISession sessao) : base(sessao)
        {
        }

        public ConfiguracaoEmailDTO BuscarUnicaConfiguracai()
        {
            var query = Sessao.Query<ConfiguracaoEmailDTO>();

            return (ConfiguracaoEmailDTO)query.FirstOrNull();
        }
    }
}