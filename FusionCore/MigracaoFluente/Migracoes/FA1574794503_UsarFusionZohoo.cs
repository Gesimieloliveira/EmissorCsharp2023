using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574794503)]
    public class FA1574794503_UsarFusionZohoo : Migration
    {
        public override void Up()
        {
            Alter.Table("configuracao_email")
                .AddColumn("usarFusionZohoo").AsBoolean().NotNullable().WithDefaultValue(true);

            Delete.DefaultConstraint().OnTable("configuracao_email").InSchema("dbo").OnColumn("usarFusionZohoo");
        }

        public override void Down()
        {
        }
    }
}