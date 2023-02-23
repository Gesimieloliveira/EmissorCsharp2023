using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionServico;
using FusionCore.Servicos.Core.Exportacao;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Servicos.Core.Repositorios
{
    public class RepositorioNfe : IRepositorioExportacao
    {
        private readonly IStatelessSession _sessao;
        private readonly Nfeletronica _tbNfe = null;
        private readonly EmissaoFinalizadaNfe _tbEmissao = null;
        private readonly CancelamentoNfe _tbCancelamento = null;
        private readonly DocumentoXml _documentoXml = null;
        private readonly NfeExportada _tbNfeExportada = null;

        public RepositorioNfe(IStatelessSession sessao)
        {
            _sessao = sessao;
        }

        public IList<DocumentoXml> ListarDocumentosNaoExportados()
        {
            var query = _sessao.QueryOver(() => _tbNfe)
                .SelectList(list => list
                    .Select(() => _tbEmissao.Nfe.Id).WithAlias(() => _documentoXml.Id)
                    .Select(() => _tbEmissao.Chave.Chave).WithAlias(() => _documentoXml.Chave)
                    .Select(() => _tbEmissao.XmlAutorizado).WithAlias(() => _documentoXml.Xml)
                    .Select(() => _tbEmissao.RecebidoEm).WithAlias(() => _documentoXml.Recebimento)
                    .Select(() => _tbEmissao.TipoAmbiente).WithAlias(() => _documentoXml.Ambiente)
                    .Select(() => _tbCancelamento.Status.Codigo).WithAlias(() => _documentoXml.CodigoCancelamento))
                .JoinAlias(() => _tbNfe.Finalizacao, () => _tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => _tbNfe.Cancelamento, () => _tbCancelamento, JoinType.LeftOuterJoin);

            query.WithSubquery.WhereProperty(() => _tbNfe.Id)
                .NotIn(QueryOver.Of(() => _tbNfeExportada).Select(i => i.NfeId));

            query.TransformUsing(Transformers.AliasToBean<DocumentoXml>());

            return query.List<DocumentoXml>();
        }

        public void SalvarExportadas(IEnumerable<DocumentoXml> exportados)
        {
            using (var transaction = _sessao.BeginTransaction())
            {
                _sessao.SetBatchSize(25);

                foreach (var doc in exportados)
                {
                    _sessao.Insert(new NfeExportada(doc.Id));
                }

                transaction.Commit();
            }
        }

        public void Dispose()
        {
            _sessao?.Dispose();
        }
    }
}