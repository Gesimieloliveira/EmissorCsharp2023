using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.FusionAdm.PedidoVenda.Repositorio
{
    public class RepositorioPedidoVendaPreferencia : Repositorio<PedidoVendaPreferencia, int>
    {
        public RepositorioPedidoVendaPreferencia(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(PedidoVendaPreferencia preferencia)
        {
            Sessao.SaveOrUpdate(preferencia);
        }

        public PedidoVendaPreferencia GetPeloIdentificadorMaquina(string identificador)
        {
            var query = Sessao.QueryOver<PedidoVendaPreferencia>()
                .Where(i => i.IdentificadorMaquina == identificador);

            return query.SingleOrDefault();
        }

        public bool PossuiPreferenciaParaMaquina(string identificador)
        {
            var query = Sessao.QueryOver<PedidoVendaPreferencia>()
                .Where(i => i.IdentificadorMaquina == identificador);

            return query.RowCount() > 0;
        }
    }
}