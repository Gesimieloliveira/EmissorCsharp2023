using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.EstoqueEvento
{
    public class EventosPeloProduto : IBuscaListagem<EstoqueEventoDTO>
    {
        private readonly ProdutoDTO _produto;

        public EventosPeloProduto(ProdutoDTO produto)
        {
            _produto = produto;
        }

        public IList<EstoqueEventoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<EstoqueEventoDTO>()
                .Where(ee => ee.ProdutoDTO == _produto);

            return query.ToList();
        }
    }
}