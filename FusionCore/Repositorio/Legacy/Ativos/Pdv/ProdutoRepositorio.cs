using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class ProdutoRepositorio : RepositorioBase<ProdutoDt>
    {
        public ProdutoRepositorio(ISession sessao)
            : base(sessao)
        {
        }

        public ProdutoDt BuscarPorCodigoBarraOuCodigo(string texto)
        {
            try
            {
                var search = texto ?? string.Empty;

                var produtoAliasBuscado = Sessao.QueryOver<ProdutoAliasDt>()
                    .Where(Restrictions.Eq(Projections.Property<ProdutoAliasDt>(p => p.Alias), search)
                    ).SingleOrDefault<ProdutoAliasDt>();


                var produto = produtoAliasBuscado?.Produto ?? Sessao.QueryOver<ProdutoDt>()
                    .Where(Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property<ProdutoDt>(p => p.Id)), search))
                    .SingleOrDefault<ProdutoDt>();

                if(produto != null)
                    NHibernateUtil.Initialize(produto.ProdutosAlias);

                return produto;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }
        }

        public IList<ProdutoDt> BuscarProdutos(string texto)
        {
            ProdutoDt produto = null;
            ProdutoAliasDt produtoAliasNfce = null;

            if (string.IsNullOrWhiteSpace(texto))
                return Sessao.QueryOver<ProdutoDt>().List();

            var whereEstoqueDTO = Restrictions.Disjunction();

            whereEstoqueDTO.Add(
                Restrictions.Like(
                    Projections.Property(() => produto.Nome),
                    texto, MatchMode.Anywhere));

            whereEstoqueDTO.Add(
                Restrictions.Eq(
                    Projections.Cast(NHibernateUtil.String, Projections.Property(() => produto.Id)),
                    texto));

            whereEstoqueDTO.Add(
                Restrictions.Eq(Projections.Property(() => produtoAliasNfce.Alias), texto));


            var queryOver = Sessao.QueryOver(() => produto)
                .JoinAlias(() => produto.ProdutosAlias, () => produtoAliasNfce, JoinType.LeftOuterJoin)
                .Where(whereEstoqueDTO);

            var lista = queryOver.List();

            return lista;
        }

        public void AcrescentaEstoque(ProdutoDt produto, decimal quantidade)
        {
            try
            {
                Sessao.Evict(produto);
                Sessao.Evict(produto.ProdutosAlias);

                produto = Sessao.Get<ProdutoDt>(produto.Id);
                NHibernateUtil.Initialize(produto.ProdutosAlias);

                produto.Estoque += quantidade;
                Alterar(produto);
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public void DescontaEstoque(ProdutoDt produto, decimal quantidade)
        {
            try
            {
                Sessao.Evict(produto);
                Sessao.Evict(produto.ProdutosAlias);

                produto = Sessao.Get<ProdutoDt>(produto.Id);
                NHibernateUtil.Initialize(produto.ProdutosAlias);

                produto.Estoque -= quantidade;
                Alterar(produto);
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public bool PossuiSaldoDisponivel(ProdutoDt produto, decimal quantidade)
        {
            try
            {
                Sessao.Evict(produto);
                Sessao.Evict(produto.ProdutosAlias);
                var produtoAtualizado = BuscaPorId(produto.Id);

                return produtoAtualizado.Estoque >= quantidade;
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public ProdutoDt BuscaPorCodigoBalanca(string codigoBarra)
        {
            ProdutoDt produto;

            try
            {
                var queryOver = Sessao.QueryOver<ProdutoDt>()
                    .Where(Restrictions.Eq(Projections.Cast(NHibernateUtil.Int32, Projections.Property<ProdutoDt>(p => p.CodigoBalanca)), 
                    codigoBarra));

                produto = queryOver.SingleOrDefault();

                if (produto != null)
                {
                    NHibernateUtil.Initialize(produto.ProdutosAlias);
                }
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }

            return produto;
        }

        public void DeletarProdutoAlias(ProdutoDt produto)
        {
            var query = Sessao.CreateQuery("delete from " + nameof(ProdutoAliasDt) + " where Produto.Id = :produto");
            query.SetInt32("produto", produto.Id);
            query.ExecuteUpdate();
        }
    }
}