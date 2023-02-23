using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1587564356)]
    public class FA1587564356_AdicionarQuantidadeTributavelProduto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto").AddColumn("quantidadeUnidadeTributavel").AsDecimal(15, 4).WithDefaultValue(0.0m).NotNullable();
            Delete.DefaultConstraint().OnTable("produto").InSchema("dbo").OnColumn("quantidadeUnidadeTributavel");
        }

        public override void Down()
        {
        }
    }
}