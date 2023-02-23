using FusionCore.FusionAdm.Pessoas;
using NHibernate;

namespace Fusion.Conversor.Core.Repositorios.CustomQueries
{
    public static class TabelaFornecedor
    {
        public static void InsertFornecedor(IStatelessSession session, PessoaEntidade pessoa)
        {
            var sql = $"INSERT INTO {CustomQuery.TbPessoaFornecedor}(pessoa_id) VALUES(:pId)";

            var query = session.CreateSQLQuery(sql)
                .SetParameter("pId", pessoa.Id);

            query.ExecuteUpdate();
        }
    }
}