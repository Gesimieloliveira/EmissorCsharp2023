using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1590433155)]
    public class FN1590433155_OpcaoAdicionarVendedor : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal")
                .AddColumn("opcaoAdicionarVendedor").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("preferencia_terminal").InSchema("dbo").OnColumn("opcaoAdicionarVendedor");
        }

        public override void Down()
        {
        }
    }
}