using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace FusionCore.Tributacoes.Repositorio
{
    public class RepositorioIpi : RepositorioBase
    {
        public RepositorioIpi(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<TributacaoIpi> TodosParaSaida()
        {
            var query = Sessao.QueryOver<TributacaoIpi>()
                .Where(i => i.TipoOperacao == TipoOperacao.Saida);

            return query.List();
        }
    }
}