using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.NfeInutilizacaoNumeracao
{
    public class TodasNfeInutilizacaoNumeracao : IBuscaListagem<NfeInutilizacaoNumeracaoDTO>
    {
        public IList<NfeInutilizacaoNumeracaoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<NfeInutilizacaoNumeracaoDTO>();

            return query.ToList();
        }
    }
}
