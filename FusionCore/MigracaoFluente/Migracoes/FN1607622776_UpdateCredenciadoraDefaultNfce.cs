using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1607622776)]
    public class FN1607622776_UpdateCredenciadoraDefaultNfce : Migration
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