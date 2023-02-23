using FusionCore.FusionAdm.PedidoVenda;
using NHibernate;

namespace Fusion.FastReport.Repositorios
{
    public class RepositorioPedido : Repositorio
    {
        public RepositorioPedido(IStatelessSession sessao) : base(sessao)
        {
        }

        public EstadoAtual GetEstadoDoPedido(int pedidoId)
        {
            var q = Sessao.QueryOver<PedidoVenda>()
                .Select(i => i.EstadoAtual)
                .Where(i => i.Id == pedidoId);

            return q.SingleOrDefault<EstadoAtual>();
        }
    }
}