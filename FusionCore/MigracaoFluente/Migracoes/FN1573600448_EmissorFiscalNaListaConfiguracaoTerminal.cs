using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1573600448)]
    public class FN1573600448_EmissorFiscalNaListaConfiguracaoTerminal : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update emissor_fiscal set emissor_fiscal.terminalOffline_id = t.terminalOffline_id 
                from emissor_fiscal ef
                inner join configuracao_terminal t
                on t.emissorFiscal_id = ef.id");

            Alter.Column("emissorFiscal_id").OnTable("configuracao_terminal").InSchema("dbo")
                .AsByte().Nullable();
        }

        public override void Down()
        {
        }
    }
}