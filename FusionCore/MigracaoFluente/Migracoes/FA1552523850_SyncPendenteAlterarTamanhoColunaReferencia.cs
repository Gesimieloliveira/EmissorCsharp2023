using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552523850)]
    public class FA1552523850_SyncPendenteAlterarTamanhoColunaReferencia : Migration
    {
        public override void Up()
        {
            Alter.Table("sync_pendente").InSchema("dbo")
                .AlterColumn("referencia").AsAnsiString(100).NotNullable();
        }

        public override void Down()
        {
        }
    }
}