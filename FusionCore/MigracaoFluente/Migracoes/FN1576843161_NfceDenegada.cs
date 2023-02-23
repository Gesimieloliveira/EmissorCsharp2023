using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1576843161)]
    public class FN1576843161_NfceDenegada : Migration
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