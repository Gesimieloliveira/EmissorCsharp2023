using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1573586665)] 
    public class FA1573586665_UpdateTerminal : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update emissor_fiscal set emissor_fiscal.terminalOffline_id = t.id 
                from emissor_fiscal ef
                inner join terminal_offline t
                on t.emissorFiscal_id = ef.id");

            Alter.Column("emissorFiscal_id").OnTable("terminal_offline").InSchema("dbo")
                .AsByte().Nullable();
        }

        public override void Down()
        {
        }
    }
}