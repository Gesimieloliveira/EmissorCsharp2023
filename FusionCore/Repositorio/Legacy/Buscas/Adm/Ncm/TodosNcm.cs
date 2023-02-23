using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Ncm
{
    public class TodosNcm : IBuscaListagem<NcmDTO>
    {
        public IList<NcmDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<NcmDTO>();
            return query.ToList();
        }
    }
}