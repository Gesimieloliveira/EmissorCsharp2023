using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1554986405)]
    public class FA1554986405_DeletaRestricaoAgenda : Migration
    {
        public override void Up()
        {
            Delete.Column("restricaoAgenda").FromTable("usuario");
        }

        public override void Down()
        {
        }
    }
}