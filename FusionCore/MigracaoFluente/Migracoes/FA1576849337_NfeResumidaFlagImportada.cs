using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1576849337)]
    public class FA1576849337_NfeResumidaFlagImportada : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_resumo").AddColumn("isImportada").AsBoolean().NotNullable().WithDefaultValue(false);
            Delete.DefaultConstraint().OnTable("mde_resumo").InSchema("dbo").OnColumn("isImportada");
        }

        public override void Down()
        {
        }
    }
}