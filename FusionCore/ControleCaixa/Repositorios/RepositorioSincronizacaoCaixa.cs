using System.Collections.Generic;
using FusionCore.NfceSincronizador.ControleCaixa;
using NHibernate;

namespace FusionCore.ControleCaixa.Repositorios
{
    public class RepositorioSincronizacaoCaixa
    {
        private readonly ISession _session;

        public RepositorioSincronizacaoCaixa(ISession session)
        {
            _session = session;
        }

        public IEnumerable<SyncLancamentoCaixa> ObterLançamentosPendentes()
        {
            var query = _session.QueryOver<SyncLancamentoCaixa>();

            return query.List();
        }

        public IEnumerable<SyncCaixaIndividual> ObterCaixasPendentes()
        {
            var query = _session.QueryOver<SyncCaixaIndividual>();

            return query.List();
        }
    }
}