using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Sincronizacao
{
    public class BuscaFormaPagamento : IBuscaListagem<PdvFormaPagamentoDTO>
    {
        public IList<PdvFormaPagamentoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<PdvFormaPagamentoDTO>();
            return query.ToList();
        }
    }
}