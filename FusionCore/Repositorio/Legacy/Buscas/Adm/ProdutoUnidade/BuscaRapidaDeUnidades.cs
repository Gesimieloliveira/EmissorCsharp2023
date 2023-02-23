using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoUnidade
{
    public class BuscaRapidaDeUnidades : IBuscaListagem<ProdutoUnidadeDTO>
    {
        private readonly string _filtro;

        public BuscaRapidaDeUnidades(string filtro)
        {
            _filtro = filtro;
        }

        public IList<ProdutoUnidadeDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<ProdutoUnidadeDTO>()
                .Where(u => u.Id.ToString() == _filtro || u.Nome.Contains(_filtro) || u.Sigla == _filtro);

            return query.ToList();
        }
    }
}