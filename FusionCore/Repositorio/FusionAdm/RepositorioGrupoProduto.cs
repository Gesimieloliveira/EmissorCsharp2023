using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioGrupoProduto : Repositorio<ProdutoGrupoDTO, int>
    {
        public RepositorioGrupoProduto(ISession sessao) : base(sessao)
        {
        }

        public ProdutoGrupoDTO GetFirstGrupo()
        {
            var sessao = Sessao.QueryOver<ProdutoGrupoDTO>().Take(1);
            return sessao.SingleOrDefault();
        }

        public IList<ProdutoGrupoDTO> BuscarTodosOrdenado(Expression<Func<ProdutoGrupoDTO, object>> ordenacao)
        {
            var query = Sessao.QueryOver<ProdutoGrupoDTO>()
                .OrderBy(ordenacao).Asc;

            return query.List<ProdutoGrupoDTO>();
        }
    }
}