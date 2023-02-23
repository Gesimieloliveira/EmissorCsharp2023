using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1675304614)]
    public class FN1675304614_ResponsavelTecnicoNfceAddTodos : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete from responsavel_tecnico
                            GO");
            Execute.Sql(@"insert into responsavel_tecnico (guid, csrt, uf_id, csrtId) select NEWID(), '', uf.id, 0 from uf
                        GO");
        }

        public override void Down()
        {
        }
    }
}