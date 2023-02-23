using NHibernate;
using NHibernate.Criterion;
using NHibernate.Persister.Entity;

namespace Fusion.Conversor.Core.Repositorios.CustomQueries
{
    public static class CustomQuery
    {
        public const string TbProduto = "dbo.produto";
        public const string TbPessoa = "dbo.pessoa";
        public const string TbPessoaCliente = "dbo.pessoa_cliente";
        public const string TbPessoaFornecedor = "dbo.pessoa_fornecedor";
        public const string TbEstoque = "dbo.produto_estoque";
        public const string TbEventoEstoque = "dbo.produto_estoque_evento";

        public static void ActiveInsertIdentity(IStatelessSession session, params string[] tables)
        {
            foreach (var table in tables)
            {
                session.CreateSQLQuery($"SET IDENTITY_INSERT {table} ON;").ExecuteUpdate();
            }
        }

        public static void ResetIdentity<TEntity>(IStatelessSession session, string identityName)
        {
            var entity = typeof(TEntity);

            var metadata = session
                .GetSessionImplementation()
                .Factory.GetClassMetadata(entity) as AbstractEntityPersister;

            var tbName = metadata.TableName;

            var criteria = session.CreateCriteria(entity).SetProjection(Projections.Max(identityName));

            var result = criteria.UniqueResult<int>();
            var lastId = result == 0 ? 1 : result;

            session.CreateSQLQuery($"DBCC CHECKIDENT ('{tbName}', RESEED, {lastId})").ExecuteUpdate();
        }
    }
}