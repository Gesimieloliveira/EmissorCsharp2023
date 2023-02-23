using System.Collections.Generic;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Flags;
using NHibernate;

namespace FusionCore.Tributacoes.Repositorio
{
    public class RepositorioTributacaoCst : Repositorio<TributacaoCst, string>
    {
        public RepositorioTributacaoCst(ISession sessao) : base(sessao)
        {
        }

        public override IList<TributacaoCst> BuscaTodos()
        {
            var q = Sessao.QueryOver<TributacaoCst>()
                .OrderBy(i => i.RegimeTributario).Asc
                .OrderBy(i => i.Id).Asc;

            return q.List();
        }

        public IEnumerable<TributacaoCst> ParaNfe(RegimeTributario regime)
        {
            var regimeQuery = regime == RegimeTributario.SimplesNacional
                ? RegimeTributario.SimplesNacional
                : RegimeTributario.RegimeNormal;

            var query = Sessao.QueryOver<TributacaoCst>()
                .Where(i => i.RegimeTributario == regimeQuery);

            return query.List();
        }
    }
}