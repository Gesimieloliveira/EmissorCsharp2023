using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Tributacoes.Federal;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.SituacaoTributaria.Cofins
{
    public class TodosCofins : IBuscaListagem<TributacaoCofins>
    {
        public IList<TributacaoCofins> Busca(ISession sessao)
        {
            var query = sessao.Query<TributacaoCofins>();
            return query.ToList();
        }
    }
}