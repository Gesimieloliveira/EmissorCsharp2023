using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551198032)]
    public class FA1551198032_ProdutoAdicionaReducaoIcms : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"insert into sync_pendente (referencia, terminalOffline_id, entidade)
                select p.id, t.id, 2 from terminal_offline as t cross join produto as p where p.reducaoIcms != 0 
                and p.id not in (select s.referencia from sync_pendente as s where s.entidade = 2 and s.terminalOffline_id = t.id)");
        }

        public override void Down()
        {
        }
    }
}