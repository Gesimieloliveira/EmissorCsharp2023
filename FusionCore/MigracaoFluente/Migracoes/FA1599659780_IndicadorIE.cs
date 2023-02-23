using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1599659780)]
    public class FA1599659780_IndicadorIE : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update pessoa set indicadorIe = 1 where inscricaoEstadual != '' and inscricaoEstadual != 'ISENTO';");
        }

        public override void Down()
        {
        }
    }
}