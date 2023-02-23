using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1587566541)]
    public class FA1587566541_QuantidadeUnidadeTributavelItemNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_item").AddColumn("quantidadeUnidadeTributavel").AsDecimal(15, 4).NotNullable().WithDefaultValue(0);
            Delete.DefaultConstraint().OnTable("nfe_item").InSchema("dbo").OnColumn("quantidadeUnidadeTributavel");
        }

        public override void Down()
        {
        }
    }
}