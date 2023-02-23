using System.Collections.Generic;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Util;

namespace FusionCore.FusionAdm.TabelasDePrecos.NfceSync
{
    public class RepositorioTabelaPrecoNfce : Repositorio<TabelaPrecoNfce, int>, IRepositorioAjusteDiferenciado
    {
        public RepositorioTabelaPrecoNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salva(TabelaPrecoNfce tabelaPreco)
        {
            Sessao.SaveOrUpdate(tabelaPreco);

            tabelaPreco.AjusteDiferenciadoLista.ForEach(ajusteDiferenciadoNfce =>
            {
                Sessao.SaveOrUpdate(ajusteDiferenciadoNfce);
            });
        }


        public void Deleta(TabelaPrecoNfce tabelaPreco)
        {
            Sessao.Delete(tabelaPreco);
        }

        public void Deleta(AjusteDiferenciadoNfce ajusteDiferenciadoNfce)
        {
            Sessao.Delete(ajusteDiferenciadoNfce);
        }

        public IEnumerable<TabelaPrecoDto> BuscarTodasTabelasDto()
        {
            TabelaPrecoDto reposta = null;
            TabelaPrecoNfce tabelaPrecoNfce = null;

            var queryOver = Sessao.QueryOver(() => tabelaPrecoNfce)
                .Where(t => t.Status != false)
                .SelectList(lista => lista
                    .Select(() => tabelaPrecoNfce.Id).WithAlias(() => reposta.Id)
                    .Select(() => tabelaPrecoNfce.Descricao).WithAlias(() => reposta.Descricao));

            queryOver.TransformUsing(Transformers.AliasToBean<TabelaPrecoDto>());
            var result = queryOver.List<TabelaPrecoDto>();

            return result;
        }

        public AjusteDiferenciadoDto BuscarAjusteDiferenciado(int produtoId, int tabelaPrecoId)
        {
            TabelaPrecoNfce tabelaPrecoNfce = null;
            AjusteDiferenciadoNfce tabelaAjusteDiferenciadoNfce = null;
            ProdutoNfce produtoAlias = null;
            AjusteDiferenciadoDto resposta = null;

            var queryOver = Sessao.QueryOver(() => tabelaPrecoNfce)
                .JoinAlias(() => tabelaPrecoNfce.AjusteDiferenciadoLista, () => tabelaAjusteDiferenciadoNfce)
                .JoinAlias(() => tabelaAjusteDiferenciadoNfce.Produto, () => produtoAlias)
                .SelectList(lista
                    => lista.Select(() => tabelaPrecoNfce.TipoAjustePreco).WithAlias(() => resposta.TipoAjustePreco)
                        .Select(() => tabelaAjusteDiferenciadoNfce.PercentualAjuste).WithAlias(() => resposta.PercentualAjuste));


            queryOver.TransformUsing(Transformers.AliasToBean<AjusteDiferenciadoDto>());

            queryOver.Where(Restrictions.Eq(Projections.Property(() => produtoAlias.Id), produtoId));
            queryOver.Where(Restrictions.Eq(Projections.Property(() => tabelaPrecoNfce.Id), tabelaPrecoId));

            return queryOver.SingleOrDefault<AjusteDiferenciadoDto>();

        }
    }
}