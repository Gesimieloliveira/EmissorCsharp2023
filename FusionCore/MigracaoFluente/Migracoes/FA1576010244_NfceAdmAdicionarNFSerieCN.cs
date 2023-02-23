using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1576010244)]
    public class FA1576010244_NfceAdmAdicionarNFSerieCN : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("numeroFiscal").AsInt32().NotNullable().WithDefaultValue(0);
            Alter.Table("nfce").AddColumn("serie").AsInt16().NotNullable().WithDefaultValue(0);
            Alter.Table("nfce").AddColumn("codigoNumerico").AsInt32().NotNullable().WithDefaultValue(0);

            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("numeroFiscal");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("serie");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("codigoNumerico");
        }

        public override void Down()
        {
        }
    }
}