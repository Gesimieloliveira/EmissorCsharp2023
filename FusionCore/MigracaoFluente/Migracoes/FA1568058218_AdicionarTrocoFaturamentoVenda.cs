using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1568058218)]
    public class FA1568058218_AdicionarTrocoFaturamentoVenda : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_venda")
                .AddColumn("troco").AsDecimal(15, 2).WithDefaultValue(0.0m);

            Delete.DefaultConstraint().OnTable("faturamento_venda").InSchema("dbo").OnColumn("troco");
        }

        public override void Down()
        {
        }
    }
}