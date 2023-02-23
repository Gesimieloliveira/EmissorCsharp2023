using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Cidade
{
    public class CidadesPorSiglaUF : IBuscaListagem<CidadeDTO>
    {
        private readonly string _siglaUF;

        public CidadesPorSiglaUF(string siglaUF)
        {
            _siglaUF = siglaUF;
        }

        public IList<CidadeDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<CidadeDTO>()
                .Where(c => c.SiglaUf.ToLower().Equals(_siglaUF.ToLower()));

            return query.ToList();
        }
    }
}