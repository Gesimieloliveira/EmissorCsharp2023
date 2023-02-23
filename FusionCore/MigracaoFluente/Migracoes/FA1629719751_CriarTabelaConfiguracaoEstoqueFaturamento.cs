using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1629719751)]
    public class FA1629719751_CriarTabelaConfiguracaoEstoqueFaturamento : Migration
    {
        public override void Up()
        {
            Create.Table("configuracao_estoque_faturamento")
                .WithColumn("id").AsGuid().PrimaryKey("pk_configuracao_estoque_faturamento")
                .WithColumn("movimentarEstoque").AsBoolean().NotNullable();
        }

        public override void Down()
        {
        }
    }
}