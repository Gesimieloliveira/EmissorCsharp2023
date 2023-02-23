using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1574797464)]
    public class FN1574797464_ConfiguracaoServidorEmailProprio : Migration
    {
        public override void Up()
        {
            Alter.Table("configuracao_email")
                .AddColumn("usarFusionZohoo").AsBoolean().NotNullable().WithDefaultValue(true);

            Delete.DefaultConstraint().OnTable("configuracao_email").InSchema("dbo").OnColumn("usarFusionZohoo");

            Alter.Table("configuracao_email")
                .AddColumn("emailResposta").AsAnsiString(255).NotNullable().WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("configuracao_email").InSchema("dbo").OnColumn("emailResposta");
        }

        public override void Down()
        {
        }
    }
}