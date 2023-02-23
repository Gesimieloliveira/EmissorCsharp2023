using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoUnidade
{
    public class TodasUnidades : IBuscaListagem<ProdutoUnidadeDTO>
    {
        public IList<ProdutoUnidadeDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<ProdutoUnidadeDTO>();
            return query.ToList();
        }
    }
}