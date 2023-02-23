using System;
using FusionCore.FusionAdm.PedidoVenda;
using NHibernate;
using static NHibernate.Criterion.Projections;
using static NHibernate.Criterion.Restrictions;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroListagemPedido : IFiltro
    {
        private readonly PedidoVenda _tbPedido = null;

        public bool Abertos { get; set; }
        public bool Finalizados { get; set; }
        public bool Ultimos7Dias { get; set; }
        public bool ApenasPedidos { get; set; }

        public void Aplicar<TRoot, TSub>(IQueryOver<TRoot, TSub> queryover)
        {
            var or = Disjunction();
            var and = Conjunction();

            if (ApenasPedidos)
            {
                var eqTipo = Eq(Property(() => _tbPedido.TipoPedido), TipoPedido.PedidoVenda);
                and.Add(eqTipo);
            }

            if (Abertos)
            {
                var restriction = Eq(Property(() => _tbPedido.EstadoAtual), EstadoAtual.Aberto);
                or.Add(restriction);
            }

            if (Finalizados)
            {
                var restriction = Eq(Property(() => _tbPedido.EstadoAtual), EstadoAtual.Finalizado);
                or.Add(restriction);
            }

            if (Ultimos7Dias)
            {
                var cast = Cast(NHibernateUtil.Date, Property(() => _tbPedido.AbertoEm));
                var ge = Ge(cast, DateTime.Now.AddDays(-7));

                or.Add(ge);
            }

            queryover.Where(and && or);
        }
    }
}