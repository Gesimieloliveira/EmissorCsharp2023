using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("RR")]
    [Migration(1597773297)]
    public class RR1597773297_DeletarRelatoriosFiscaisNfceAntigos : Migration
    {
        public override void Up()
        {
            Execute.Sql("update Relatorios set Relatorios.Deleted = 1, Relatorios.FolderId = -2 where Relatorios.ItemId = 18");
            Execute.Sql("update Relatorios set Relatorios.Deleted = 1, Relatorios.FolderId = -2 where Relatorios.ItemId = 19");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}