using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1590520057)]
    public class FA1590520057_AdicionaVendedorSincronizacaoNfce : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete sync_pendente where entidade = 3");
            Execute.Sql(@"insert into sync_pendente (referencia, terminalOffline_id, entidade) select CAST(p.id as varchar(100)), tt.id, 3 from terminal_offline as tt, pessoa p");
        }

        public override void Down()
        {
        }
    }
}