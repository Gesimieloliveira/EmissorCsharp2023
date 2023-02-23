using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574796599)]
    public class FA1574796599_EmailResposta : Migration
    {
        public override void Up()
        {
            Alter.Table("configuracao_email")
                .AddColumn("emailResposta").AsAnsiString(255).NotNullable().WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("configuracao_email").InSchema("dbo").OnColumn("emailResposta");
        }

        public override void Down()
        {
            
        }
    }
}