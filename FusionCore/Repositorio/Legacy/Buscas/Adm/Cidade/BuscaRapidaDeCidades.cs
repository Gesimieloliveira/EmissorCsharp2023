using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Cidade
{
    public class BuscaRapidaDeCidades : IBuscaListagem<CidadeDTO>
    {
        private readonly string _texto;

        public BuscaRapidaDeCidades(string texto)
        {
            _texto = texto;
        }

        public IList<CidadeDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<CidadeDTO>()
                .Where(c => c.Nome.Contains(_texto)
                            || c.CodigoIbge.ToString() == _texto
                            || c.SiglaUf == _texto
                            || c.Id.ToString() == _texto);

            return query.ToList();
        }
    }
}