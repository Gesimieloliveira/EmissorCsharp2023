using FusionCore.FusionAdm.Pessoas;
using NHibernate;

namespace Fusion.Conversor.Core.Repositorios.CustomQueries
{
    public static class TabelaCliente
    {
        public static int UltimoCodigo(IStatelessSession session)
        {
            var q = session.QueryOver<PessoaEntidade>()
                .Select(i => i.Id)
                .OrderBy(i => i.Id).Desc
                .Take(1);

            return q.SingleOrDefault<int>();
        }

        public static void InsertCliente(IStatelessSession session, PessoaEntidade pessoa, string observacao)
        {
            observacao = string.IsNullOrWhiteSpace(observacao) ? string.Empty : observacao;

            var sql =
                $"INSERT INTO {CustomQuery.TbPessoaCliente}(pessoa_id, aplicaLimiteCredito, limiteCredito, observacao, solicitaPedido)" +
                " VALUES(:pId, 0, 0, :obs, 0)";

            var query = session.CreateSQLQuery(sql)
                .SetParameter("pId", pessoa.Id)
                .SetParameter("obs", observacao);

            query.ExecuteUpdate();
        }
    }
}