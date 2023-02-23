using System.Collections.Generic;
using FusionCore.FusionAdm.Tef;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Contratos;
using NHibernate;
using NHibernate.Criterion;
using Restrictions = NHibernate.Criterion.Restrictions;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPos : Repositorio<Pos, short>, IRepositorioPos
    {
        public RepositorioPos(ISession sessao) : base(sessao)
        {
        }

        public void SalvarOuAtualizar(Pos pos)
        {
            Sessao.SaveOrUpdate(pos);
        }

        public IEnumerable<Pos> BuscaComFiltro(string textoPesquisado)
        {
            var filtro = textoPesquisado.TrimOrEmpty();

            if (filtro.IsNullOrEmpty())
                return BuscaTodos();

            var dijunction = Restrictions.Disjunction();

            dijunction.Add(Restrictions.Like(Projections.Property<Pos>(x => x.Descricao), filtro, MatchMode.Anywhere));

            dijunction.Add(Restrictions.Eq(Projections.Property<Pos>(x => x.Serial), filtro));

            dijunction.Add(Restrictions.Eq(Projections.Property<Pos>(x => x.EstabelecimentoCodigo), filtro));

            dijunction.Add(Restrictions.Like(Projections.Property<Pos>(x => x.Adquirente), filtro, MatchMode.Anywhere));

            var queryOver = Sessao.QueryOver<Pos>()
                .Where(dijunction);

            var lista = queryOver.OrderBy(p => p.Id).Asc.List();

            return lista;
        }
    }
}