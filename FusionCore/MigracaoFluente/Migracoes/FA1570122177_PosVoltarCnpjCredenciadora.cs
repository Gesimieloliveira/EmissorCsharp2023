using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1570122177)]
    public class FA1570122177_PosVoltarCnpjCredenciadora : Migration
    {
        public override void Up()
        {
            Alter.Table("pos").AddColumn("cnpjCredenciadora").AsAnsiString(14).NotNullable().WithDefaultValue(string.Empty);
            Alter.Table("pos").AddColumn("flagMfe").AsBoolean().NotNullable().WithDefaultValue(true);
            Alter.Table("pos").AddColumn("flagNfce").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("pos").InSchema("dbo").OnColumn("cnpjCredenciadora");
            Delete.DefaultConstraint().OnTable("pos").InSchema("dbo").OnColumn("flagMfe");
            Delete.DefaultConstraint().OnTable("pos").InSchema("dbo").OnColumn("flagNfce");
        }

        public override void Down()
        {
        }
    }
}