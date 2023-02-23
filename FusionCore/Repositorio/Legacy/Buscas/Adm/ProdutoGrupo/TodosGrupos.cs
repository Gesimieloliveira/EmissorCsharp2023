using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoGrupo
{
    public class TodosGrupos : IBuscaListagem<ProdutoGrupoDTO>
    {
        public IList<ProdutoGrupoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<ProdutoGrupoDTO>();
            return query.ToList();
        }
    }
}