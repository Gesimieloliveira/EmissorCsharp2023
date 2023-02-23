using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1607622721)]
    public class FA1607622721_UpdateCredenciadoraDefaultParaAdm : Migration
    {
        public override void Up()
        {
            Execute.Sql("update pos set pos.credenciadora = 999 where pos.flagMfe = 1");
        }

        public override void Down()
        {
        }
    }
}