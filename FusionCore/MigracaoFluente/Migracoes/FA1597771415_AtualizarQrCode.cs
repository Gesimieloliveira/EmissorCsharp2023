using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1597771415)]
    public class FA1597771415_AtualizarQrCode : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update configuracao_dfe
                set 
                isQrCodeCte = 1
                , isQrCodeCteOs = 1
                , isQrCodeMdfe = 1");
        }

        public override void Down()
        {
        }
    }
}