using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("RR")]
    [Migration(1581358125)]
    public class RR1581358125_DeletarRelatoriosFiscaisAntigos : Migration
    {
        public override void Up()
        {
            Execute.Sql("update Relatorios set Relatorios.Deleted = 1, Relatorios.FolderId = -2 where Relatorios.ItemId = 40");
            Execute.Sql("update Relatorios set Relatorios.Deleted = 1, Relatorios.FolderId = -2 where Relatorios.ItemId = 35");
            Execute.Sql("update Relatorios set Relatorios.Deleted = 1, Relatorios.FolderId = -2 where Relatorios.ItemId = 39");
        }

        public override void Down()
        {
        }
    }
}