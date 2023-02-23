using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;

namespace Fusion.Conversor.Core.Repositorios.CustomQueries
{
    public static class TabelaEstoque
    {
        public static void InsertEstoque(IStatelessSession session, int produtoId, decimal estoque)
        {
            var sql =
                $"INSERT INTO {CustomQuery.TbEstoque}(produto_id, estoque, estoqueReservado, alteradoEm, estoqueMinimo, estoqueMaximo)" +
                " VALUES(:pId, :estoque, 0, getdate(), 0, 0)";

            var query = session.CreateSQLQuery(sql)
                .SetParameter("pId", produtoId)
                .SetParameter("estoque", estoque);

            query.ExecuteUpdate();
        }

        public static void RegistrarEvento(IStatelessSession sessao, int produtoId, decimal quantidade)
        {
            var sql =
                $"INSERT INTO {CustomQuery.TbEventoEstoque}(produto_id, tipoEvento, tipoEventoTexto, origemEvento, origemEventoTexto, origemEventoDetalhe, estoqueAtual, movimento, estoqueFuturo, usuario_id, cadastradoEm)" + 
                " VALUES(:pId, :tipoEv, :evTexto, :origemEv, :origemEvTexto, :origemEvDetalhe, :estAtual, :mov, :estFuturo, :userId, GETDATE())";

            var query = sessao.CreateSQLQuery(sql)
                .SetParameter("pId", produtoId)
                .SetParameter("tipoEv", TipoEventoEstoque.Entrada)
                .SetParameter("evTexto", TipoEventoEstoque.Entrada.GetDescription())
                .SetParameter("origemEv", OrigemEventoEstoque.SaldoInicial)
                .SetParameter("origemEvTexto", "SaldoInicial")
                .SetParameter("origemEvDetalhe", "Saldo inicial adicionado por migração de dados")
                .SetParameter("estAtual", 0.00M)
                .SetParameter("mov", quantidade)
                .SetParameter("estFuturo", quantidade)
                .SetParameter("userId", 1);

            query.ExecuteUpdate();
        }
    }
}