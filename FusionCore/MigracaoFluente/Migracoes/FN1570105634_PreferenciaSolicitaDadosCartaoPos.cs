using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1570105634)]
    public class FN1570105634_PreferenciaSolicitaDadosCartaoPos : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal")
                .AddColumn("solicitaDadosCartaoPos").AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable("preferencia_terminal").InSchema("dbo").OnColumn("solicitaDadosCartaoPos");
        }

        public override void Down()
        {
        }
    }
}