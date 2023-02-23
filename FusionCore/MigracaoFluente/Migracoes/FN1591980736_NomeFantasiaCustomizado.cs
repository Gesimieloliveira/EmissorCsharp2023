using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1591980736)]
    public class FN1591980736_NomeFantasiaCustomizado : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal")
                .AddColumn("nomeFantasiaCustomizado").AsAnsiString(60).NotNullable().WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("preferencia_terminal").InSchema("dbo").OnColumn("nomeFantasiaCustomizado");
        }

        public override void Down()
        {
        }
    }
}