using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1573600811)]
    public class FN1573600811_DeletaEmissorFiscalIdDeConfiguracaoTerminal : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("fk_terminal_offline__emissor_fiscal").OnTable("configuracao_terminal");
            Delete.Column("emissorFiscal_id").FromTable("configuracao_terminal");
        }

        public override void Down()
        {
        }
    }
}