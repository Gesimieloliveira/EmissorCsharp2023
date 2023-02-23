using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.EstoqueEvento
{
    public class TodosEventos : IBuscaListagem<EstoqueEventoDTO>
    {
        public IList<EstoqueEventoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<EstoqueEventoDTO>();
            return query.ToList();
        }
    }
}