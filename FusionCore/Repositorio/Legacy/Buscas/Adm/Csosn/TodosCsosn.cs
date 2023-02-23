using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Csosn
{
    public class TodosCsosn : IBuscaListagem<CsosnDTO>
    {
        public IList<CsosnDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<CsosnDTO>();

            return query.ToList();
        }
    }
}
