using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1571686531)]
    public class FN1571686531_NfceComIntegradorFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("emissor_fiscal_nfce").AddColumn("isIntegradorCeara").AsBoolean().WithDefaultValue(false);
            Delete.DefaultConstraint().OnTable("emissor_fiscal_nfce").InSchema("dbo").OnColumn("isIntegradorCeara");
        }

        public override void Down()
        {
        }
    }
}