using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionWPF.Parcelamento;
using NHibernate;

namespace Fusion.Parcelamento
{
    public class RepositorioParcelamento : IRepositorioParcelamento
    {
        private readonly ISession _session;

        public RepositorioParcelamento(ISession session)
        {
            _session = session;
        }

        public void Dispose()
        {
            _session.Close();
        }

        public IEnumerable<ITipoDocumento> BuscaTiposDocumentos()
        {
            var q = _session.QueryOver<TipoDocumento>();
            return q.List();
        }
    }
}