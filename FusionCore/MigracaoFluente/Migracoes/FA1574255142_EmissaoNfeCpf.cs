using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574255142)]
    public class FA1574255142_EmissaoNfeCpf : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_historico_emissao")
                .AddColumn("cpf").AsAnsiString(11).NotNullable().WithDefaultValue(string.Empty);
            Delete.DefaultConstraint().OnTable("nfe_historico_emissao").InSchema("dbo").OnColumn("cpf");

            Alter.Table("nfe_emissao")
                .AddColumn("cpf").AsAnsiString(11).NotNullable().WithDefaultValue(string.Empty);
            Delete.DefaultConstraint().OnTable("nfe_emissao").InSchema("dbo").OnColumn("cpf");
        }

        public override void Down()
        {
        }
    }
}