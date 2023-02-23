using System.Collections.Generic;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Estadual;
using NHibernate;

namespace FusionCore.Tributacoes.Repositorio
{
    public class RepositorioIcms : RepositorioBase
    {
        public RepositorioIcms(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<TributacaoIcms> TodosParaNfe()
        {
            var q = Sessao.QueryOver<TributacaoIcms>()
                .Where(i => i.IsNFe == true);

            return q.List();
        }
    }
}