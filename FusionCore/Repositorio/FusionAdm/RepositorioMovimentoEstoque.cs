using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Estoque.Movimentacoes;
using FusionCore.Repositorio.Contratos;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioMovimentoEstoque : Repositorio<MovimentoEstoque, int>, IRepositorioMovimentoEstoque
    {
        public RepositorioMovimentoEstoque(ISession sessao) : base(sessao)
        {
        }

        public MovimentoEstoque GetPeloId(int id, bool loadLazy = true)
        {
            if (loadLazy == false)
            {
                return base.GetPeloId(id);
            }

            var movimento = base.GetPeloId(id);
            if (movimento != null) NHibernateUtil.Initialize(movimento);

            return movimento;
        }

        public IList<MovimentoEstoque> BuscaRapida(string input)
        {
            var query = Sessao.QueryOver<MovimentoEstoque>()
                .OrderBy(l => l.Id).Desc;

            if (string.IsNullOrWhiteSpace(input))
            {
                return query.List();
            }

            var w1 = Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property<MovimentoEstoque>(m => m.Id)), input);
            var w2 = Restrictions.InsensitiveLike(Projections.Property<MovimentoEstoque>(m => m.Descricao), input, MatchMode.Anywhere);

            query.Where(Restrictions.Or(w1, w2));

            return query.List();
        }

        public MovimentoEstoque Persiste(MovimentoEstoque movimento)
        {
            Sessao.Persist(movimento);
            Sessao.Flush();

            return movimento;
        }

        public MovimentoEstoque Altera(MovimentoEstoque movimento)
        {
            Sessao.Update(movimento);
            Sessao.Flush();

            return movimento;
        }

        public void Deletar(MovimentoEstoque movimento)
        {
            if (movimento.Itens.Count > 0)
                throw new InvalidOperationException("Movimento Estoque possui itens. Não pode ser excluido!");

            Sessao.Delete(movimento);
            Sessao.Flush();
        }
    }
}