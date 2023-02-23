using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf
{
    public class TodosEcf : IBuscaListagem<PdvEcfDTO>
    {
        public IList<PdvEcfDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<PdvEcfDTO>();
            return query.ToList();
        }
    }
}