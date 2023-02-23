using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1560776476)]
    public class FA1560776476_RemovendoPosNfce : Migration
    {
        public override void Up()
        {
            Execute.Sql("delete from pos where flagNfce = 1");
            Delete.Column("cnpjCredenciadora").FromTable("pos");
            Delete.Column("flagMfe").FromTable("pos");
            Delete.Column("flagNfce").FromTable("pos");
        }

        public override void Down()
        {
        }
    }
}