using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1591984599)]
    public class FN1591984599_AdicionarOpcaoNaoImprimir : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal").AddColumn("naoImprimir").AsBoolean().WithDefaultValue(false).NotNullable();
            Delete.DefaultConstraint().OnTable("preferencia_terminal").InSchema("dbo").OnColumn("naoImprimir");
        }

        public override void Down()
        {
        }
    }
}