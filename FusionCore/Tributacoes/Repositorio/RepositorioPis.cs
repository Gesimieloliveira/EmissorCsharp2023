using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace FusionCore.Tributacoes.Repositorio
{
    public class RepositorioPis : RepositorioBase
    {
        public RepositorioPis(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<TributacaoPis> TodosParaNfe(TipoOperacao operacao)
        {
            var q = Sessao.QueryOver<TributacaoPis>()
                .Where(i => i.TipoOperacao == operacao);

            return q.List();
        }
    }
}