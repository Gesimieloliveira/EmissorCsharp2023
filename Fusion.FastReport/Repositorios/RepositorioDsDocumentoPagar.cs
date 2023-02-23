using System;
using System.Collections.Generic;
using Fusion.FastReport.DataSources;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Fusion.FastReport.Repositorios
{
    public class RepositorioDsDocumentoPagar : Repositorio
    {
        private readonly DsDocumentosPagar _ds;
        private readonly DocumentoPagar _documento = null;
        private readonly Fornecedor _fornecedor = null;
        private readonly PessoaEntidade _pessoa = null;

        public RepositorioDsDocumentoPagar(IStatelessSession sessao) : base(sessao)
        {
        }

        public IList<DsDocumentosPagar> BuscaAbertosAteDataFinal(DateTime final)
        {
            var query = Sessao.QueryOver(() => _documento)
                .JoinAlias(() => _documento.Fornecedor, () => _fornecedor, JoinType.InnerJoin)
                .JoinAlias(() => _fornecedor.Pessoa, () => _pessoa, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => _documento.Id).WithAlias(() => _ds.Id)
                    .Select(() => _pessoa.Nome).WithAlias(() => _ds.NomeRecebedor)
                    .Select(() => _documento.NumeroDocumento).WithAlias(() => _ds.NumeroDocumento)
                    .Select(() => _documento.Parcela).WithAlias(() => _ds.Parcela)
                    .Select(() => _documento.ValorAjustado).WithAlias(() => _ds.ValorAjustado)
                    .Select(() => _documento.ValorOriginal).WithAlias(() => _ds.ValorOriginal)
                    .Select(() => _documento.ValorQuitado).WithAlias(() => _ds.ValorQuitado)
                    .Select(() => _documento.Vencimento).WithAlias(() => _ds.Vencimento)
                );

            query.TransformUsing(Transformers.AliasToBean<DsDocumentosPagar>());

            var conjunction = Restrictions.Conjunction();

            conjunction.Add(Restrictions.Eq(Projections.Property(() => _documento.Situacao), Situacao.Aberto));
            conjunction.Add(Restrictions.Le(Projections.Property(() => _documento.Vencimento), final.Date));

            query.Where(conjunction);

            return query.List<DsDocumentosPagar>();
        }
    }
}