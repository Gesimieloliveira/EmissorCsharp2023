using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1586278250)]
    public class FA1586278250_NfeSemPagamento : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe").AddColumn("semPagamento").AsBoolean().WithDefaultValue(false).NotNullable();
            Delete.DefaultConstraint().OnTable("nfe").InSchema("dbo").OnColumn("semPagamento");
        }

        public override void Down()
        {
        }
    }
}