using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1571683802)]
    public class FA1571683802_NfceComIntegradorFiscal : Migration
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