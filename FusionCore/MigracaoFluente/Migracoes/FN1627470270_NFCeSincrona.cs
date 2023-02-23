using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1627470270)]
    public class FN1627470270_NFCeSincrona : Migration
    {
        public override void Up()
        {
            Execute.Sql("update configuracao_transmissao_nfce set transmissao = 0");
        }

        public override void Down()
        {
        }
    }
}