using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Cidade
{
    public class TodasCidades : IBuscaListagem<CidadeDTO>
    {
        public IList<CidadeDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<CidadeDTO>();
            return query.ToList();
        }
    }
}