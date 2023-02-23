using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552656350)]
    public class FA1552656350_AdicionarSiglaUnidadeTributavelNfceItem : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_item")
                .AddColumn("siglaUnidadeTributavel").AsAnsiString(10).WithDefaultValue(string.Empty).NotNullable();

            Delete.DefaultConstraint().OnTable("nfce_item").InSchema("dbo").OnColumn("siglaUnidadeTributavel");
        }

        public override void Down()
        {
        }
    }
}