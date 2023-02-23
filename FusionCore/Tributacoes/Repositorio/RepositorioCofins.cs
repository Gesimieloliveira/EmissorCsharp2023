using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace FusionCore.Tributacoes.Repositorio
{
    public class RepositorioCofins : RepositorioBase
    {
        public RepositorioCofins(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<TributacaoCofins> TodosParaNfe(TipoOperacao operacao)
        {
            var q = Sessao.QueryOver<TributacaoCofins>()
                .Where(i => i.TipoOperacao == operacao);

            return q.List();
        }
    }
}