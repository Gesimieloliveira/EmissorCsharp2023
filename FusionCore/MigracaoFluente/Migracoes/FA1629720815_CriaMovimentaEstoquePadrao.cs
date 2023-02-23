using FluentMigrator;
using FusionCore.Configuracoes;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1629720815)]
    public class FA1629720815_CriaMovimentaEstoquePadrao : Migration
    {
        public override void Up()
        {
            Execute.Sql($"insert into configuracao_estoque_faturamento (id, movimentarEstoque) values ('{ConfiguracaoEstoqueFaturamento.IdStatic}', 1)");
        }

        public override void Down()
        {
        }
    }
}