using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail
{
    public class UnicaConfiguracaoEmail : IBuscaUnico<ConfiguracaoEmailDTO>
    {
        public ConfiguracaoEmailDTO Busca(ISession sessao)
        {
            var query = sessao.Query<ConfiguracaoEmailDTO>();

            return (ConfiguracaoEmailDTO) query.FirstOrNull();
        }
    }
}
