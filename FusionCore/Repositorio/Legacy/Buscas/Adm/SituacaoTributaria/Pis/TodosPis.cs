using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Tributacoes.Federal;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.SituacaoTributaria.Pis
{
    public class TodosPis : IBuscaListagem<TributacaoPis>
    {
        public IList<TributacaoPis> Busca(ISession sessao)
        {
            var query = sessao.Query<TributacaoPis>();
            return query.ToList();
        }
    }
}