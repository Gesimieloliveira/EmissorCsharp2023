using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1587574635)]
    public class FN1587574635_AdicionarQuantidadeUnidadeTributavel : Migration
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