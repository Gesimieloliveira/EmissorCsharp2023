using System.Collections.Generic;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionServico;
using FusionCore.Servicos.Core.Exportacao;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Servicos.Core.Repositorios
{
    public class RepositorioNfce : IRepositorioExportacao
    {
        private readonly IStatelessSession _sessao;
        private readonly NfceAdm _tbNfce = null;
        private readonly NfceEmissaoAdm _tbEmissao = null;
        private readonly NfceCancelamentoAdm _tbCancelamento = null;
        private readonly NfceExportada _tbNfceExportada = null;
        private readonly DocumentoXml _documentoXml = null;

        public RepositorioNfce(IStatelessSession sessao)
        {
            _sessao = sessao;
        }

        public IList<DocumentoXml> ListarDocumentosNaoExportados()
        {
            var query = _sessao.QueryOver(() => _tbNfce)
                .SelectList(list => list
                    .Select(() => _tbEmissao.NfceId).WithAlias(() => _documentoXml.Id)
                    .Select(() => _tbEmissao.Chave).WithAlias(() => _documentoXml.Chave)
                    .Select(() => _tbEmissao.XmlAutorizado).WithAlias(() => _documentoXml.Xml)
                    .Select(() => _tbEmissao.RecebidoEm).WithAlias(() => _documentoXml.Recebimento)
                    .Select(() => _tbEmissao.TipoAmbiente).WithAlias(() => _documentoXml.Ambiente)
                    .Select(() => _tbCancelamento.StatusRetorno).WithAlias(() => _documentoXml.CodigoCancelamento))
                .JoinAlias(() => _tbNfce.Emissao, () => _tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => _tbNfce.Cancelamento, () => _tbCancelamento, JoinType.LeftOuterJoin);

            query.Where(() => _tbEmissao.Autorizado == true);

            query.WithSubquery.WhereProperty(() => _tbNfce.Id)
                .NotIn(QueryOver.Of(() => _tbNfceExportada).Select(i => i.NfceId));

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
                    _sessao.Insert(new NfceExportada(doc.Id));
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