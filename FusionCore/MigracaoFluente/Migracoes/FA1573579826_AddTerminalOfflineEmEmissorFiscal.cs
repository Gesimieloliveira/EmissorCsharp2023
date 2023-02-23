using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1573579826)]
    public class FA1573579826_AddTerminalOfflineEmEmissorFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("emissor_fiscal")
                .AddColumn("terminalOffline_id").AsByte().Nullable()
                .ForeignKey("fk_emissor_fiscal__terminal_offline", "terminal_offline", "id");
        }

        public override void Down()
        {
        }
    }
}