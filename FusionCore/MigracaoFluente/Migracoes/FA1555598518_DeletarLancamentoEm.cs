using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1555598518)]
    public class FA1555598518_DeletarLancamentoEm : Migration
    {
        public override void Up()
        {
            Delete.Column("lancamentoEm").FromTable("nf_compra").InSchema("dbo");
        }

        public override void Down()
        {
        }
    }
}