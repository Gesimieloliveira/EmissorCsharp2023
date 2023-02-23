using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1675295241)]
    public class FA1675295241_ResponsavelTecnicoInserirParaTodosEstados : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete from responsavel_tecnico
                            GO");
            Execute.Sql(@"insert into responsavel_tecnico (guid, csrt, uf_id, isCTe, isCTeOs, isMDFe, isNFCe, isNFe, csrtId) select NEWID(), '', uf.id, 1, 1, 1, 1, 1, 0 from uf
                        GO");
            
        }

        public override void Down()
        {
        }
    }
}