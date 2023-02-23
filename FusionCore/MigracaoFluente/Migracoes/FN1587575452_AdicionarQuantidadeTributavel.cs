using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1587575452)]
    public class FN1587575452_AdicionarQuantidadeTributavel : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_item").AddColumn("quantidadeUnidadeTributavel").AsDecimal(15, 4).NotNullable().WithDefaultValue(0);
            Delete.DefaultConstraint().OnTable("nfce_item").InSchema("dbo").OnColumn("quantidadeUnidadeTributavel");
        }

        public override void Down()
        {
        }
    }
}