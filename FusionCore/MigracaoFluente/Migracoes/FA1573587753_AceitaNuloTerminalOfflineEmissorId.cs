using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1573587753)]
    public class FA1573587753_AceitaNuloTerminalOfflineEmissorId : Migration
    {
        public override void Up()
        {
            Execute.Sql("update terminal_offline set emissorFiscal_id = null");
            Delete.ForeignKey("fk_terminal_offline__emissor_fiscal").OnTable("terminal_offline");
            Delete.Column("emissorFiscal_id").FromTable("terminal_offline");
        }

        public override void Down()
        {
        }
    }
}