using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1576843296)]
    public class FA1576843296_NfceDenegada : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("denegada").AsBoolean().NotNullable().WithDefaultValue(false);
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("denegada");
        }

        public override void Down()
        {
        }
    }
}