using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioProdutoNfce : Repositorio<ProdutoNfce, int>
    {
        public RepositorioProdutoNfce(ISession sessao) : base(sessao)
        {
        }

        public override ProdutoNfce GetPeloId(int id)
        {
            var result =  base.GetPeloId(id);

            if (result == null)
            {
                return null;
            }

            NHibernateUtil.Initialize(result.ProdutosAlias);

            return result;
        }

        public void Salvar(ProdutoNfce produto)
        {
            Sessao.SaveOrUpdate(produto);
        }

        public IList<ProdutoNfce> BuscaPorCodigoOuCodigoBarras(string codigoOuCodigoBarras)
        {
            ProdutoAliasNfce alias = null;
            ProdutoNfce produto = null;

            var queryBarras = Sessao.QueryOver(() => produto)
                .JoinAlias(() => produto.ProdutosAlias, () => alias, JoinType.InnerJoin)
                .Where(Restrictions.Eq(Projections.Property(() => alias.Alias), codigoOuCodigoBarras));

            var produtos = queryBarras.List<ProdutoNfce>();

            if (produtos.Count > 0)
            {
                return produtos;
            }

            var propertyId = Projections.Cast(NHibernateUtil.String, Projections.Property(() => produto.Id));

            var queryId = Sessao.QueryOver(() => produto)
                .Where(Restrictions.Eq(propertyId, codigoOuCodigoBarras));

            return queryId.List<ProdutoNfce>();
        }

        public IList<ProdutoNfce> BuscarPorCodigoBarrasComZeroAEsquerda(string codigoBarras)
        {
            ProdutoAliasNfce alias = null;
            ProdutoNfce produto = null;

            var queryBarras = Sessao.QueryOver(() => produto)
                .JoinAlias(() => produto.ProdutosAlias, () => alias, JoinType.InnerJoin)
                .Where(Restrictions.Eq(Projections.Property(() => alias.Alias), codigoBarras));

            var produtos = queryBarras.List<ProdutoNfce>();

            return produtos;
        }

        public ProdutoNfce BuscaPorCodigoBalanca(int codigoBalanca)
        {
            ProdutoNfce produto = null;

            var propertyCodigoBalanca = Projections.Property(() => produto.CodigoBalanca);

            var queryCodigoBalanca = Sessao.QueryOver(() => produto)
                .Where(Restrictions.Eq(propertyCodigoBalanca, codigoBalanca));

            return queryCodigoBalanca.SingleOrDefault<ProdutoNfce>();
        }

        public void AcrescentaEstoque(ProdutoNfce produto, decimal movimento)
        {
            Sessao.Evict(produto);
            produto = GetPeloId(produto.Id);
            produto.Estoque += movimento;
            Sessao.SaveOrUpdate(produto);
        }

        public void DescontaEstoque(ProdutoNfce produto, decimal movimento)
        {
            Sessao.Evict(produto);
            produto = GetPeloId(produto.Id);
            produto.Estoque -= movimento;
            Sessao.SaveOrUpdate(produto);
        }

        public IList<ProdutoNfce> BuscaRapidaProduto(string search)
        {
            ProdutoNfce produto = null;
            ProdutoAliasNfce aliases = null;

            if (string.IsNullOrWhiteSpace(search))
                return Sessao.QueryOver<ProdutoNfce>().Take(2000).List<ProdutoNfce>();

            var disjunction = Restrictions.Disjunction();

            disjunction.Add(
                Restrictions.Like(
                    Projections.Property(() => produto.Nome),
                    search,
                    MatchMode.Anywhere));

            disjunction.Add(
                Restrictions.Eq(
                    Projections.Cast(NHibernateUtil.String, Projections.Property(() => produto.Id)),
                    search));

            disjunction.Add(Restrictions.Eq(Projections.Property(() => aliases.Alias), search));
            disjunction.Add(Restrictions.Eq(Projections.Property(() => produto.Referencia), search));

            var queryOver = Sessao.QueryOver(() => produto)
                .JoinAlias(() => produto.ProdutosAlias, () => aliases, JoinType.LeftOuterJoin)
                .Where(disjunction)
                .Take(2000);

            var result = queryOver.List();

            return result.Distinct().ToList();
        }

        public void Refresh(ProdutoNfce produto)
        {
            Sessao.Refresh(produto);
            NHibernateUtil.Initialize(produto.ProdutosAlias);
        }

        public void DeletarProdutoAlias(int idProduto)
        {
            var query = Sessao.CreateQuery("delete from " + nameof(ProdutoAliasNfce) + " where Produto.Id = :produto");
            query.SetInt32("produto", idProduto);
            query.ExecuteUpdate();
            Sessao.Flush();
        }

        public ProdutoCodigoAnpNfce BuscaPorCodigoAnp(string id)
        {
            ProdutoCodigoAnpNfce codigoAnpNfce = null;

            var queryBuscaCodigoAnp = Sessao.QueryOver(() => codigoAnpNfce);

            var whereIgualId = Restrictions.Eq(Projections.Property(() => codigoAnpNfce.Id), id);

            queryBuscaCodigoAnp.Where(whereIgualId);

            var produtoCodigoAnp = queryBuscaCodigoAnp.SingleOrDefault();

            return produtoCodigoAnp;

        }

        public void AlterarEstoquePara(int produtoId, decimal saldo)
        {
            var query = Sessao.CreateSQLQuery("update produto set estoque = :est where id = :id");

            query.SetParameter("est", saldo);
            query.SetParameter("id", produtoId);

            query.ExecuteUpdate();
            
            Sessao.Flush();
        }

        public IEnumerable<ProdutoBaseDTO> BuscaProdutosAtivos(int limit, string nomeOuCodigo)
        {
            var query = MontaBuscaBaseGridF6(limit);

            ProdutoNfce tbProduto = null;

            if (int.TryParse(nomeOuCodigo, out var intValue))
            {
                var pId = Projections.Property(() => tbProduto.Id);
                var r = Restrictions.Eq(pId, intValue);

                query.And(r);

                return query.List<ProdutoBaseDTO>();
            }

            var pNome = Projections.Property(() => tbProduto.Nome);
            var wLikeNome = Restrictions.Like(pNome, nomeOuCodigo, MatchMode.Anywhere);

            query.And(wLikeNome);

            return query.List<ProdutoBaseDTO>();
        }

        public IEnumerable<ProdutoBaseDTO> BuscaProdutoAlias(int limit, string input)
        {
            if (string.IsNullOrWhiteSpace(input)) 
            {
                return new List<ProdutoBaseDTO>();
            }

            var query = MontaBuscaBaseGridF6(limit);

            ProdutoNfce tbProduto = null;
            ProdutoAliasNfce tbAlias = null;

            query.JoinAlias(() => tbProduto.ProdutosAlias, () => tbAlias, JoinType.InnerJoin);

            var pAlias = Projections.Property(() => tbAlias.Alias);
            var igual = Restrictions.Eq(pAlias, input);
            query.And(igual);


            return query.List<ProdutoBaseDTO>();
        }

        private IQueryOver<ProdutoNfce, ProdutoNfce> MontaBuscaBaseGridF6(int limit)
        {
            ProdutoNfce tbProduto = null;
            ProdutoUnidadeNfce tbUnidade = null;
            ProdutoBaseDTO alias = null;

            var query = Sessao.QueryOver(() => tbProduto)
                .JoinAlias(() => tbProduto.UnidadeMedida, () => tbUnidade, JoinType.InnerJoin)
                .OrderBy(() => tbProduto.Nome).Asc
                .SelectList(list => list
                    .Select(() => tbProduto.Id).WithAlias(() => alias.Id)
                    .Select(() => tbProduto.Nome).WithAlias(() => alias.Nome)
                    .Select(() => tbProduto.Referencia).WithAlias(() => alias.Referencia)
                    .Select(() => tbProduto.Ncm).WithAlias(() => alias.CodigoNcm)
                    .Select(() => tbProduto.Estoque).WithAlias(() => alias.Estoque)
                    .Select(() => tbProduto.PrecoVenda).WithAlias(() => alias.PrecoVenda)
                    .Select(() => tbUnidade.Sigla).WithAlias(() => alias.SiglaUnidade)
                );

            query.Where(i => i.Ativo == true);

            query.TransformUsing(Transformers.AliasToBean<ProdutoBaseDTO>());
            query.Take(limit);
            return query;
        }

        public void Salvar(ProdutoAliasNfce produtoAlias)
        {
            Sessao.Save(produtoAlias);
        }

        public IEnumerable<ProdutoBaseDTO> BuscarPorReferencia(int limit, string input)
        {
            var query = MontaBuscaBaseGridF6(limit);

            ProdutoNfce tbProduto = null;

            var pReferencia = Projections.Property(() => tbProduto.Referencia);
            var wLikeReferencia = Restrictions.Like(pReferencia, input, MatchMode.Anywhere);

            query.And(wLikeReferencia);

            return query.List<ProdutoBaseDTO>();
        }
    }
}