using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552653657)]
    public class FA1552653657_AdicionarSiglaUnidadeTributavel : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_item_mercadoria")
                .AddColumn("siglaUnidadeTributavel").AsAnsiString(10).WithDefaultValue(string.Empty).NotNullable();

            Delete.DefaultConstraint().OnTable("nfe_item_mercadoria").InSchema("dbo").OnColumn("siglaUnidadeTributavel");
        }

        public override void Down()
        {
        }
    }
}