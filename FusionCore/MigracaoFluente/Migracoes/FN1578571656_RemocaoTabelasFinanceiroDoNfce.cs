using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1578571656)]
    public class FN1578571656_RemocaoTabelasFinanceiroDoNfce : Migration
    {
        public override void Up()
        {
            Delete.Table("documento_receber_evento");
            Delete.Table("documento_receber");
            Delete.Table("malote");
        }

        public override void Down()
        {
        }
    }
}