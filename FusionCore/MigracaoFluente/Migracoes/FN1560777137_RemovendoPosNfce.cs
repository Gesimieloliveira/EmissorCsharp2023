using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1560777137)]
    public class FN1560777137_RemovendoPosNfce : Migration
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