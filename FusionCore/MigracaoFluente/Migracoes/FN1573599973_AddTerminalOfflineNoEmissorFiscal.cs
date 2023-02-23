using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1573599973)]
    public class FN1573599973_AddTerminalOfflineNoEmissorFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("emissor_fiscal")
                .AddColumn("terminalOffline_id").AsByte().Nullable();

            Alter.Table("emissor_fiscal")
                .AddColumn("emUso").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("emissor_fiscal").InSchema("dbo")
                .OnColumn("emUso");
        }

        public override void Down()
        {
        }
    }
}