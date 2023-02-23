using System;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.Pessoas;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroGridPedido : IFiltro
    {
        private readonly PedidoVenda _tbPedido = null;
        private readonly PessoaEntidade _tbPessoa = null;

        public EstadoAtual? EstadoAtual { get; set; }
        public DateTime? CriadoApos { get; set; }
        public int? NumeroIgual { get; set; }
        public string NomeClienteContem { get; set; }
        public string ReferenciaDocumentoContem { get; set; }

        public void Aplicar<TRoot, TSub>(IQueryOver<TRoot, TSub> queryover)
        {
            var and = Restrictions.Conjunction();

            if (EstadoAtual != null)
            {
                var eq = Restrictions.Eq(Projections.Property(() => _tbPedido.EstadoAtual), EstadoAtual);
                and.Add(eq);
            }

            if (CriadoApos != null)
            {
                var property = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => _tbPedido.AbertoEm));
                var ge = Restrictions.Ge(property, CriadoApos);

                and.Add(ge);
            }

            if (NumeroIgual != null && NumeroIgual != 0)
            {
                var eq = Restrictions.Eq(Projections.Property(() => _tbPedido.Id), NumeroIgual);
                and.Add(eq);
            }

            if (!string.IsNullOrWhiteSpace(NomeClienteContem))
            {
                var pNome = Projections.Property(() => _tbPessoa.Nome);
                var like = Restrictions.Like(pNome, NomeClienteContem, MatchMode.Anywhere);

                and.Add(like);
            }

            if (!string.IsNullOrWhiteSpace(ReferenciaDocumentoContem))
            {
                var cRef = Projections.Property(() => _tbPedido.Referencia);
                var like = Restrictions.Like(cRef, ReferenciaDocumentoContem, MatchMode.Anywhere);

                and.Add(like);
            }

            queryover.Where(and);
        }
    }
}