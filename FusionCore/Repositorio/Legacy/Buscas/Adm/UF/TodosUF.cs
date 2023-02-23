using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.UF
{
    public class TodosUF : IBuscaListagem<EstadoDTO>
    {
        public IList<EstadoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<EstadoDTO>();
            return query.ToList();
        }
    }
}