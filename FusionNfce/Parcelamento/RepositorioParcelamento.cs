using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionWPF.Parcelamento;
using NHibernate;

namespace FusionNfce.Parcelamento
{
    public class RepositorioParcelamento : IRepositorioParcelamento
    {
        private readonly ISession _session;

        public RepositorioParcelamento(ISession session)
        {
            _session = session;
        }

        public IEnumerable<ITipoDocumento> BuscaTiposDocumentos()
        {
            var query = _session.QueryOver<TipoDocumento>()
                .Where(i => i.EstaAtivo == true);

            return query.List();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}