using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Cfop
{
    public class TodosCfop : IBuscaListagem<CfopDTO>
    {
        public IList<CfopDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<CfopDTO>();

            return query.ToList();
        }
    }
}
