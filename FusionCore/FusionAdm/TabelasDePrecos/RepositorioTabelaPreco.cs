using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class RepositorioTabelaPreco : Repositorio<TabelaPreco, int>,
        IRepositorioAjusteDiferenciado
    {
        public RepositorioTabelaPreco(ISession sessao) : base(sessao)
        {
        }

        public void SalvaOuAtualiza(TabelaPreco tabelaPreco)
        {
            Sessao.SaveOrUpdate(tabelaPreco);
        }

        public void SalvaOuAtualiza(AjusteDiferenciado ajusteDiferenciado)
        {
            Sessao.SaveOrUpdate(ajusteDiferenciado);
        }

        public IList<TabelaPreco> PesquisarTabelasDePrecos(string descricao)
        {
            TabelaPreco tabelaPreco = null;
            TabelaPreco tabelaPrecoResposta = null;

            var queryOver = Sessao.QueryOver(() => tabelaPreco)
                .SelectList(lista => lista
                    .Select(() => tabelaPreco.Id).WithAlias(() => tabelaPrecoResposta.Id)
                    .Select(() => tabelaPreco.Descricao).WithAlias(() => tabelaPrecoResposta.Descricao)
                    .Select(() => tabelaPreco.TipoAjustePreco).WithAlias(() => tabelaPrecoResposta.TipoAjustePreco)
                    .Select(() => tabelaPreco.PercentualAjuste).WithAlias(() => tabelaPrecoResposta.PercentualAjuste));

            if (!string.IsNullOrEmpty(descricao))
                queryOver.Where(Restrictions.InsensitiveLike(Projections.Property(() => tabelaPreco.Descricao), descricao, MatchMode.Anywhere));

            queryOver.TransformUsing(Transformers.AliasToBean<TabelaPreco>());

            return queryOver.List<TabelaPreco>();
        }

        public IEnumerable<TabelaPrecoDto> BuscarTodasTabelasDto()
        {
            TabelaPrecoDto dto = null;
            TabelaPreco tabelaPreco = null;

            var queryOver = Sessao.QueryOver(() => tabelaPreco)
                .Where(t => t.Status != false)
                .SelectList(lista => lista
                    .Select(() => tabelaPreco.Id).WithAlias(() => dto.Id)
                    .Select(() => tabelaPreco.Descricao).WithAlias(() => dto.Descricao)
                    .Select(() => tabelaPreco.ApenasItensDaLista).WithAlias(() => dto.ApenasItensDaLista)
                    .Select(() => tabelaPreco.PercentualAjuste).WithAlias(() => dto.PercentualAjuste)
                    .Select(() => tabelaPreco.Status).WithAlias(() => dto.Status)
                    .Select(() => tabelaPreco.TipoAjustePreco).WithAlias(() => dto.TipoAjustePreco)
                );

            queryOver.TransformUsing(Transformers.AliasToBean<TabelaPrecoDto>());
            var result = queryOver.List<TabelaPrecoDto>();
            return result;
        }

        public void Remover(AjusteDiferenciado ajusteDiferenciado)
        {
            Sessao.Delete(ajusteDiferenciado);
        }

        public AjusteDiferenciadoDto BuscarAjusteDiferenciado(int produtoId, int tabelaPrecoId)
        {
            TabelaPreco tabelaPreco = null;
            AjusteDiferenciado tabelaAjusteDiferenciado = null;
            ProdutoDTO produtoAlias = null;
            AjusteDiferenciadoDto resposta = null;

            var queryOver = Sessao.QueryOver(() => tabelaPreco)
                .JoinAlias(() => tabelaPreco.AjusteDiferenciadoLista, () => tabelaAjusteDiferenciado)
                .JoinAlias(() => tabelaAjusteDiferenciado.Produto, () => produtoAlias)
                .SelectList(lista => lista
                    .Select(() => tabelaPreco.TipoAjustePreco).WithAlias(() => resposta.TipoAjustePreco)
                    .Select(() => tabelaAjusteDiferenciado.PercentualAjuste).WithAlias(() => resposta.PercentualAjuste)
                );

            queryOver.Where(Restrictions.Eq(Projections.Property(() => produtoAlias.Id), produtoId));
            queryOver.Where(Restrictions.Eq(Projections.Property(() => tabelaPreco.Id), tabelaPrecoId));

            queryOver.TransformUsing(Transformers.AliasToBean<AjusteDiferenciadoDto>());

            return queryOver.SingleOrDefault<AjusteDiferenciadoDto>();
        }

        public IEnumerable<TabelaPrecoProdutoDto> BuscarPorIdProduto(int produtoId)
        {
            TabelaPrecoProdutoDto dto = null;
            TabelaPreco tabelaPreco = null;
            AjusteDiferenciado ajusteDiferenciado = null;

            var queryOver = Sessao.QueryOver(() => tabelaPreco)
                .JoinAlias(() => tabelaPreco.AjusteDiferenciadoLista, () => ajusteDiferenciado, JoinType.LeftOuterJoin)
                .Select(
                    Projections.Distinct(
                        Projections.ProjectionList()
                            .Add(Projections.Property(() => tabelaPreco.Id).WithAlias(() => dto.Id))
                            .Add(Projections.Property(() => tabelaPreco.Descricao).WithAlias(() => dto.Descricao))
                            .Add(Projections.Property(() => tabelaPreco.TipoAjustePreco).WithAlias(() => dto.TipoAjustePreco))
                            .Add(Projections.Property(() => tabelaPreco.PercentualAjuste).WithAlias(() => dto.PercentualAjuste))
                            .Add(Projections.Property(() => ajusteDiferenciado.PercentualAjuste).WithAlias(() => dto.PercentualAjusteDiferenciado))
                            .Add(Projections.Property(() => ajusteDiferenciado.Produto.Id).WithAlias(() => dto.ProdutoId))
                    ));

            queryOver.Where(() => tabelaPreco.Status == true);

            queryOver.Where(() =>
                ajusteDiferenciado.Produto.Id == produtoId ||
                tabelaPreco.ApenasItensDaLista == false);

            queryOver.TransformUsing(Transformers.AliasToBean<TabelaPrecoProdutoDto>());

            var result = queryOver.List<TabelaPrecoProdutoDto>();

            return result;
        }
    }
}