using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Produtos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;
using FusionCore.Tributacoes.Regras;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Util;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioProduto : Repositorio<ProdutoDTO, int>
    {
        private ProdutoEstoqueDTO _tbEstoque = null;
        private ProdutoDTO _tbProduto = null;
        private ProdutoGrupoDTO _tbGrupo = null;
        private ProdutoUnidadeDTO _tbUnidade = null;
        private ProdutoGrid _produtoGrid = null;
        private ProdutoDTO _produto = null;
        private ProdutoEstoqueDTO _estoqueAlias = null;
        private RegraTributacaoSaida _tbRegraTributacao = null;
        private ProdutoGridPicker _gridPicker = null;

        public RepositorioProduto(ISession sessao) : base(sessao)
        {
        }

        public override ProdutoDTO GetPeloId(int id)
        {
            var produto = base.GetPeloId(id);
            FetchLazy(produto);

            return produto;
        }

        public ProdutoEstoqueDTO GetEstoquePeloId(int produtoId)
        {
            var query = Sessao.QueryOver<ProdutoEstoqueDTO>()
                .Where(pe => pe.ProdutoDTO.Id == produtoId);

            return query.SingleOrDefault();
        }

        public ProdutoDTO BuscaPeloCodigo(string codigo)
        {
            if (codigo == null)
            {
                return null;
            }

            var produtoAlias = Sessao.QueryOver<ProdutoAlias>()
                .Where(Restrictions.Eq(Projections.Property<ProdutoAlias>(p => p.Alias), codigo))
                .SingleOrDefault<ProdutoAlias>();

            if (produtoAlias != null)
            {
                return produtoAlias.Produto;
            }

            var prodRestriction = Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property<ProdutoDTO>(p => p.Id)), codigo);

            var produto = Sessao.QueryOver<ProdutoDTO>()
                .Where(prodRestriction)
                .SingleOrDefault<ProdutoDTO>();

            FetchLazy(produto);

            return produto;
        }

        private IQueryOver<ProdutoEstoqueDTO, ProdutoEstoqueDTO> CriaQueryProdutoGrid()
        {
            var query = Sessao.QueryOver(() => _tbEstoque)
                .JoinAlias(() => _tbEstoque.ProdutoDTO, () => _tbProduto, JoinType.InnerJoin)
                .JoinAlias(() => _tbProduto.ProdutoGrupoDTO, () => _tbGrupo, JoinType.InnerJoin)
                .JoinAlias(() => _tbProduto.ProdutoUnidadeDTO, () => _tbUnidade, JoinType.InnerJoin)
                .JoinAlias(() => _tbProduto.RegraTributacaoSaida, () => _tbRegraTributacao, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => _tbProduto.Id).WithAlias(() => _produtoGrid.Id)
                    .Select(() => _tbProduto.Ativo).WithAlias(() => _produtoGrid.Ativo)
                    .Select(() => _tbProduto.Nome).WithAlias(() => _produtoGrid.Nome)
                    .Select(() => _tbProduto.ReferenciaInterna).WithAlias(() => _produtoGrid.ReferenciaInterna)
                    .Select(() => _tbProduto.PrecoCompra).WithAlias(() => _produtoGrid.PrecoCompra)
                    .Select(() => _tbProduto.PrecoCusto).WithAlias(() => _produtoGrid.PrecoCusto)
                    .Select(() => _tbProduto.MargemLucro).WithAlias(() => _produtoGrid.MargemLucro)
                    .Select(() => _tbProduto.PrecoVenda).WithAlias(() => _produtoGrid.PrecoVenda)
                    .Select(() => _tbEstoque.Estoque).WithAlias(() => _produtoGrid.Estoque)
                    .Select(() => _tbRegraTributacao.Cst.Codigo).WithAlias(() => _produtoGrid.CstIcms)
                    .Select(() => _tbProduto.AliquotaIcms).WithAlias(() => _produtoGrid.AliquotaIcms)
                    .Select(() => _tbProduto.SituacaoTributariaIpi.Codigo).WithAlias(() => _produtoGrid.CstIpi)
                    .Select(() => _tbProduto.AliquotaIpi).WithAlias(() => _produtoGrid.AliquotaIpi)
                    .Select(() => _tbProduto.Pis.Id).WithAlias(() => _produtoGrid.CstPis)
                    .Select(() => _tbProduto.AliquotaPis).WithAlias(() => _produtoGrid.AliquotaPis)
                    .Select(() => _tbProduto.Cofins.Id).WithAlias(() => _produtoGrid.CstCofins)
                    .Select(() => _tbProduto.AliquotaCofins).WithAlias(() => _produtoGrid.AliquotaCofins)
                    .Select(() => _tbUnidade.Sigla).WithAlias(() => _produtoGrid.Unidade)
                    .Select(() => _tbGrupo.Nome).WithAlias(() => _produtoGrid.Grupo)
                    .Select(() => _tbProduto.Ncm).WithAlias(() => _produtoGrid.Ncm)
                    .Select(() => _tbProduto.Cest).WithAlias(() => _produtoGrid.Cest))
                .OrderByAlias(() => _tbProduto.Nome).Asc;

            query.Take(2000);
            query.TransformUsing(Transformers.AliasToBean<ProdutoGrid>());

            return query;
        }

        public IList<ProdutoGrid> BuscaParaGridComFiltro(FiltroProdutoGridControl filtro)
        {
            var query = CriaQueryProdutoGrid();

            query.And(Restrictions.Eq(Projections.Property(() => _tbProduto.Ativo), filtro.Ativos));

            if (filtro.ContemCodigoId())
            {
                var id = Restrictions.Eq(Projections.Property(() => _tbProduto.Id), filtro.CodigoIdIgualA);
                query.Where(id);
            }

            if (filtro.ContemCodigoBarras())
            {
                var barras = Subqueries.WhereProperty(() => _tbProduto.Id).In(QueryOver.Of<ProdutoAlias>().Where(x => x.Alias == filtro.CodigoBarrasIgualA).Select(x => x.Produto.Id));
                query.And(barras);
            }

            if (filtro.ContemNome())
            {
                var nome = Restrictions.Like(Projections.Property(() => _tbProduto.Nome), filtro.NomeProdutoContenha, MatchMode.Anywhere);
                query.Where(nome);
            }

            if (filtro.ContemReferencia())
            {
                var referencia =  Restrictions.Like(Projections.Property(() => _tbProduto.ReferenciaInterna), filtro.ReferenciaContenha, MatchMode.Anywhere);
                query.And(referencia);
            }

            if (filtro.ContemGrupo())
            {
                var grupo = Restrictions.Eq(Projections.Property(() => _tbGrupo.Id), filtro.Grupo.Id);
                query.And(grupo);
            }

            return query.List<ProdutoGrid>();
        }

        public IList<ProdutoHistoricoCompra> BuscaHistoricoCompra(int produtoId)
        {
            ItemCompra item = null;
            NotaFiscalCompra compra = null;
            ProdutoUnidadeDTO un = null;
            ProdutoHistoricoCompra alias = null;
            Fornecedor fornecedor = null;
            PessoaEntidade pessoa = null;

            var query = Sessao.QueryOver(() => item)
                .JoinAlias(() => item.Unidade, () => un, JoinType.InnerJoin)
                .JoinAlias(() => item.Nota, () => compra, JoinType.InnerJoin)
                .JoinAlias(() => compra.Fornecedor, () => fornecedor, JoinType.InnerJoin)
                .JoinAlias(() => fornecedor.Pessoa, () => pessoa, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => item.Id).WithAlias(() => alias.ItemId)
                    .Select(() => compra.Id).WithAlias(() => alias.CompraId)
                    .Select(() => compra.NumeroDocumento).WithAlias(() => alias.NumeroDocumento)
                    .Select(() => pessoa.Nome).WithAlias(() => alias.Fornecedor)
                    .Select(() => item.Produto.Id).WithAlias(() => alias.ProdutoId)
                    .Select(() => compra.EmitidaEm).WithAlias(() => alias.DataEmissao)
                    .Select(() => compra.CadastradoEm).WithAlias(() => alias.DataCadastro)
                    .Select(() => item.CadastroEm).WithAlias(() => alias.ItemCadastradoEm)
                    .Select(() => item.Quantidade).WithAlias(() => alias.Quantidade)
                    .Select(() => un.Sigla).WithAlias(() => alias.UnidadeCompra)
                    .Select(() => item.ValorUnitario).WithAlias(() => alias.ValorUnitario)
                    .Select(() => item.ValorDescontoTotal).WithAlias(() => alias.DescontoTotal)
                    .Select(() => item.ValorTotal).WithAlias(() => alias.ValorTotal)
                    .Select(() => item.ValorTotalCusto).WithAlias(() => alias.ValorTotalCusto)
                    .Select(() => item.FatorConversao).WithAlias(() => alias.FatorConversao)
                    .Select(() => item.QuantidadeConversao).WithAlias(() => alias.QuantidadeConversao)
                );

            query.Where(() => item.Produto.Id == produtoId);
            query.TransformUsing(Transformers.AliasToBean<ProdutoHistoricoCompra>());

            return query.List<ProdutoHistoricoCompra>();
        }

        public IEnumerable<ProdutoGridPicker> BuscaRapida(string input, int take = 2000)
        {
            var query = CriaQueryBaseGridPicker().OrderByAlias(() => _produto.Nome).Asc;

            query.Where(Restrictions.Eq(Projections.Property(() => _produto.Ativo), true));

            query.Take(take);

            if (string.IsNullOrWhiteSpace(input))
            {
                return query.List<ProdutoGridPicker>();
            }

            var where = Restrictions.Disjunction();

            where.Add(Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property(() => _produto.Id)), input));
            where.Add(Restrictions.Like(Projections.Property(() => _produto.ReferenciaInterna), input, MatchMode.Anywhere));

            input.Split('|').ForEach(s =>
            {
                var parte = s.Trim();

                if (parte.Length > 0)
                {
                    where.Add(Restrictions.Like(Projections.Property(() => _produto.Nome), parte, MatchMode.Anywhere));
                }
            });

            query.Where(where);

            return query.List<ProdutoGridPicker>();
        }

        public IEnumerable<ProdutoGridPicker> BuscaPorCodigoBarraAlias(string input, int take = 2000)
        {
            ProdutoAlias produtoAlias = null;
            var query = CriaQueryBaseGridPicker().OrderByAlias(() => _produto.Nome).Asc;

            query.Where(Restrictions.Eq(Projections.Property(() => _produto.Ativo), true));

            query.Take(take);

            if (string.IsNullOrWhiteSpace(input))
                return query.List<ProdutoGridPicker>();

            var aliasEqual = Restrictions.Eq(Projections.Property(() => produtoAlias.Alias), input);

            var produtoAliasSubQuery = QueryOver.Of(() => produtoAlias).Where(aliasEqual)
                .Select(prodAlias => prodAlias.Produto.Id);

            query.WithSubquery.WhereProperty(() => _produto.Id).In(produtoAliasSubQuery);

            return query.List<ProdutoGridPicker>();
        }

        private IQueryOver<ProdutoEstoqueDTO, ProdutoEstoqueDTO> CriaQueryBaseGridPicker()
        {
            var query = Sessao.QueryOver(() => _estoqueAlias)
                .JoinAlias(() => _estoqueAlias.ProdutoDTO, () => _produto, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => _produto.Id).WithAlias(() => _gridPicker.Id)
                    .Select(() => _produto.Nome).WithAlias(() => _gridPicker.Nome)
                    .Select(() => _produto.ReferenciaInterna).WithAlias(() => _gridPicker.Referencia)
                    .Select(() => _produto.PrecoCompra).WithAlias(() => _gridPicker.PrecoCompra)
                    .Select(() => _produto.PrecoVenda).WithAlias(() => _gridPicker.PrecoVenda)
                    .Select(() => _estoqueAlias.Estoque).WithAlias(() => _gridPicker.Estoque));

            query.TransformUsing(Transformers.AliasToBean<ProdutoGridPicker>());

            return query;
        }

        public IEnumerable<ProdutoRegraTributacao> BuscaRegrasPorProduto(ProdutoDTO produto)
        {
            var queryOver = Sessao.QueryOver<ProdutoRegraTributacao>()
                .Where(r => r.Produto == produto);

            return queryOver.List();
        }

        public IList<ProdutoAlias> BuscarAliasesPorProduto(ProdutoDTO produto)
        {
            var queryOver = Sessao.QueryOver<ProdutoAlias>()
                .Where(r => r.Produto == produto);

            return queryOver.List();
        }

        public ProdutoRegraTributacao GetRegraTributacao(ProdutoDTO produto, string siglaUf)
        {
            var query = Sessao.QueryOver<ProdutoRegraTributacao>()
                .Where(r => r.Produto == produto);

            var regras = query.List();

            return regras.FirstOrDefault(r => r.Uf.Sigla == siglaUf);
        }

        public void Salvar(ProdutoDTO produto)
        {
            ThrowExceptionSeNaoExisteTransacao();
            ChecarCodigoBalanca(produto);

            if (produto.Id == 0)
            {
                produto.Ativo = true;
                produto.CadastradoEm = DateTime.Now;
                SalvarNovo(produto);
                return;
            }

            SalvarAlteracao(produto);
        }

        private void ChecarCodigoBalanca(ProdutoDTO produto)
        {
            if (produto.CodigoBalanca == 0)
            {
                return;
            }

            var query = Sessao.QueryOver<ProdutoDTO>()
                .Where(p => p.CodigoBalanca == produto.CodigoBalanca && p.Id != produto.Id);

            if (query.RowCount() > 0)
            {
                throw new RegraNegocioException("Código de balaça já definido para outro produto!");
            }
        }

        private void SalvarNovo(ProdutoDTO produto)
        {
            Sessao.Persist(produto);
            Sessao.Flush();

            foreach (var alias in produto.ProdutosAlias)
            {
                if (alias.Id != 0)
                {
                    continue;
                }

                alias.Produto = produto;
                Sessao.Persist(alias);
                Sessao.Flush();
            }
        }

        private void SalvarAlteracao(ProdutoDTO produto)
        {
            produto.AlteradoEm = DateTime.Now;
            Sessao.Update(produto);
            Sessao.Flush();
        }

        public void Alterar(ProdutoRegraTributacao regra)
        {
            Sessao.Update(regra);
            Sessao.Flush();
        }

        public void Persistir(ProdutoRegraTributacao regra)
        {
            Sessao.Persist(regra);
            Sessao.Flush();
        }

        public void Deletar(ProdutoRegraTributacao regra)
        {
            Sessao.Delete(regra);
            Sessao.Flush();
        }

        private ProdutoDTO FetchLazy(ProdutoDTO produto)
        {
            if (produto == null)
            {
                return null;
            }

            NHibernateUtil.Initialize(produto.RegrasInterstaduais);
            NHibernateUtil.Initialize(produto.ProdutosAlias);

            return produto;
        }

        public bool RegraJaExiste(ProdutoRegraTributacao regra)
        {
            var queryOver = Sessao.QueryOver<ProdutoRegraTributacao>()
                .Select(Projections.RowCount())
                .Where(r => r.Uf == regra.Uf && r.Produto == regra.Produto);

            return queryOver.FutureValue<int>().Value > 0;
        }

        public void SalvarAlias(ProdutoAlias pa)
        {
            if (pa.Id == 0)
            {
                Sessao.Persist(pa);
                Sessao.Flush();

                return;
            }

            Sessao.Update(pa);
            Sessao.Flush();
        }

        public void Deletar(ProdutoAlias produtoAlias)
        {
            Sessao.Delete(produtoAlias);
            Sessao.Flush();
        }

        public bool JaExisteEsseAlias(ProdutoAlias alias)
        {
            var queryOver = Sessao.QueryOver<ProdutoAlias>()
                .Select(Projections.RowCount())
                .Where(p => p.Alias == alias.Alias && p.Id != alias.Id);

            return queryOver.FutureValue<int>().Value > 0;
        }

        public new void Refresh(ProdutoDTO produto)
        {
            Sessao.Refresh(produto);
            FetchLazy(produto);
        }

        public ProdutoDTO BuscaPeloAlias(string alias, bool onlyIsBarras = false)
        {
            var query = Sessao.QueryOver<ProdutoAlias>()
                .And(i => i.Alias == alias);

            if (onlyIsBarras)
            {
                query.And(i => i.IsCodigoBarras == true);
            }

            var result = query.SingleOrDefault<ProdutoAlias>();

            return result?.Produto;
        }

        public ProdutoVinculoCompra BuscaVinculo(string codigo, string documentoFornecedor, string unidade)
        {
            var query = Sessao.QueryOver<ProdutoVinculoCompra>()
                .Where(v =>
                    v.Codigo == codigo
                    && v.DocumentoFornecedor == documentoFornecedor
                    && v.UnidadeCompra == unidade
                ).Take(1);

            return query.SingleOrDefault<ProdutoVinculoCompra>();
        }

        public void Salvar(ProdutoVinculoCompra vinculo)
        {
            if (vinculo.Id == 0)
            {
                Sessao.Persist(vinculo);
                Sessao.Flush();
                return;
            }

            Sessao.Update(vinculo);
            Sessao.Flush();
        }

        public IList<ProdutoEstoqueDTO> ParaSincronizacaoPdv(DateTime ultimaSync)
        {
            var query = Sessao.Query<ProdutoEstoqueDTO>()
                .Where(p =>
                    p.ProdutoDTO.AlteradoEm >= ultimaSync ||
                    p.AlteradoEm >= ultimaSync ||
                    p.ProdutoDTO.ProdutoUnidadeDTO.AlteradoEm >= ultimaSync);

            return query.ToList();
        }

        public decimal SaldoEstoque(ProdutoDTO produto)
        {
            return SaldoEstoque(produto.Id);
        }

        public decimal SaldoEstoque(int produtoId)
        {
            var query = Sessao.QueryOver<ProdutoEstoqueDTO>()
                .Where(p => p.ProdutoDTO.Id == produtoId)
                .Select(p => p.Estoque);

            return query.SingleOrDefault<decimal>();
        }

        public int QuantidadeProdutoSemTabelaEstoque()
        {
            ProdutoDTO produto = null;
            ProdutoEstoqueDTO estoque = null;

            var query = Sessao.QueryOver(() => estoque)
                .JoinAlias(() => estoque.ProdutoDTO, () => produto, JoinType.RightOuterJoin);

            var conjunction = Restrictions.Conjunction();

            conjunction.Add(Restrictions.IsNull(Projections.Property(() => estoque.ProdutoDTO)));

            query.Where(conjunction);

            return query.RowCount();
        }

        public ProdutoDTO BuscaPeloCodigoBalanca(int codigo)
        {
            var query = Sessao.QueryOver<ProdutoDTO>()
                .Where(i => i.CodigoBalanca == codigo);

            var produtos = query.List();

            if (produtos.Count > 1)
            {
                throw new InvalidOperationException($"Foi encontrado mais de um produto para o código da balança. Código {codigo}");
            }

            return FetchLazy(produtos.FirstOrDefault());
        }

        public bool ExisteProdutoCodigoAnp(string id)
        {
            ProdutoCodigoAnp produtoCodigoAnp = null;

            var queryQuantidadeProdutoCodigoAnp = Sessao.QueryOver(() => produtoCodigoAnp);

            var whereId = Restrictions.Eq(Projections.Property(() => produtoCodigoAnp.Id), id);

            queryQuantidadeProdutoCodigoAnp.Where(whereId);

            var qtdCodigoAnp = queryQuantidadeProdutoCodigoAnp.RowCount();

            return qtdCodigoAnp > 0;
        }

        public ProdutoCodigoAnp BuscaCodigoAnp(string codigoAnp)
        {
            var query = Sessao.QueryOver<ProdutoCodigoAnp>()
                .Where(i => i.Id == codigoAnp);

            return query.SingleOrDefault();
        }

        public IList<Registro74Dto> BuscarRegistro74Sintegra(DateTime dataInventario)
        {
            EstoqueEventoDTO estoqueEventoAlias = null;
            ProdutoDTO produtoAlias = null;
            Registro74Dto respostaAlias = null;

            var proriedadeCadastradoEm = Projections.Property(() => estoqueEventoAlias.CadastradoEm);
            var cadastradoEmMenorOuIgual = Restrictions.Le(
                Projections.Cast(NHibernateUtil.Date, proriedadeCadastradoEm),
                dataInventario);

            var subquery = QueryOver.Of(() => estoqueEventoAlias)
                .Select(estoque => estoque.Id)
                .Where(cadastradoEmMenorOuIgual)
                .Where(() => estoqueEventoAlias.ProdutoDTO.Id == produtoAlias.Id)
                .OrderByAlias(() => estoqueEventoAlias.CadastradoEm).Desc.Take(1);

            var and = Restrictions.Conjunction();
            and.Add(cadastradoEmMenorOuIgual);
            and.Add(Restrictions.Gt(Projections.Property(() => estoqueEventoAlias.EstoqueFuturo), 0));
            and.Add(Restrictions.Gt(Projections.Property(() => produtoAlias.PrecoVenda), 0));

            var query = Sessao.QueryOver(() => estoqueEventoAlias)
                .JoinAlias(() => estoqueEventoAlias.ProdutoDTO, () => produtoAlias, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => produtoAlias.Id).WithAlias(() => respostaAlias.CodigoProdutoServico)
                    .Select(() => produtoAlias.PrecoVenda).WithAlias(() => respostaAlias.ValorUnitario)
                    .Select(() => estoqueEventoAlias.EstoqueFuturo).WithAlias(() => respostaAlias.Quantidade))
                .WithSubquery.WhereProperty(() => estoqueEventoAlias.Id).Eq(subquery)
                .Where(and)

                .TransformUsing(Transformers.AliasToBean<Registro74Dto>())
                .OrderBy(x => x.ProdutoDTO.Id);

            var listaRegistro = query
                .Asc.List<Registro74Dto>();

            return listaRegistro;
        }

        public void Salvar(ProdutoEstoqueDTO estoque)
        {
            Sessao.SaveOrUpdate(estoque);
            Sessao.Flush();
            Sessao.Clear();
        }

        public IList<Registro75Dto> BuscarRegistro75Inventario(DateTime inventarioEm)
        {
            EstoqueEventoDTO estoqueEventoAlias = null;
            ProdutoDTO produtoAlias = null;
            ProdutoUnidadeDTO produtoUnidade = null;
            Registro75Dto registro75Dto = null;

            var proriedadeCadastradoEm = Projections.Property(() => estoqueEventoAlias.CadastradoEm);
            var cadastradoEmMenorOuIgual = Restrictions.Le(
                Projections.Cast(NHibernateUtil.Date, proriedadeCadastradoEm),
                inventarioEm);

            var subquery = QueryOver.Of(() => estoqueEventoAlias)
                .Select(estoque => estoque.Id)
                .Where(cadastradoEmMenorOuIgual)
                .Where(() => estoqueEventoAlias.ProdutoDTO.Id == produtoAlias.Id)
                .OrderByAlias(() => estoqueEventoAlias.CadastradoEm).Desc.Take(1);

            var and = Restrictions.Conjunction();
            and.Add(cadastradoEmMenorOuIgual);
            and.Add(Restrictions.Gt(Projections.Property(() => estoqueEventoAlias.EstoqueFuturo), 0));
            and.Add(Restrictions.Gt(Projections.Property(() => produtoAlias.PrecoVenda), 0));

            var query = Sessao.QueryOver(() => estoqueEventoAlias)
                .JoinAlias(() => estoqueEventoAlias.ProdutoDTO, () => produtoAlias, JoinType.InnerJoin)
                .JoinAlias(() => produtoAlias.ProdutoUnidadeDTO, () => produtoUnidade, JoinType.InnerJoin)
                .SelectList(list => list
                    .SelectGroup(() => produtoAlias.Id).WithAlias(() => registro75Dto.CodigoProdutoServico)
                    .SelectGroup(() => produtoAlias.Ncm).WithAlias(() => registro75Dto.CodigoNcm)
                    .SelectGroup(() => produtoAlias.Nome).WithAlias(() => registro75Dto.Descricao)
                    .SelectGroup(() => produtoUnidade.Sigla).WithAlias(() => registro75Dto.UnidadeMedida)
                    .SelectGroup(() => produtoAlias.AliquotaIpi).WithAlias(() => registro75Dto.AliquotaIpi)
                    .SelectGroup(() => produtoAlias.AliquotaIcms).WithAlias(() => registro75Dto.AliquotaIcms)
                    .SelectGroup(() => produtoAlias.ReducaoIcms).WithAlias(() => registro75Dto.ReducaoBaseCalculoIcms)
                )
                .WithSubquery.WhereProperty(() => estoqueEventoAlias.Id).Eq(subquery)
                .Where(and)

                .TransformUsing(Transformers.AliasToBean<Registro75Dto>());

            var listaRegistro = query.List<Registro75Dto>();

            return listaRegistro;
        }
    }
}