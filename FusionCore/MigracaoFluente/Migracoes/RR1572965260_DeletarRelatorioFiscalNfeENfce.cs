using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("RR")]
    [Migration(1572965260)]
    public class RR1572965260_DeletarRelatorioFiscalNfeENfce : Migration
    {
        public override void Up()
        {
            Execute.Sql("update Relatorios set Relatorios.Deleted = 1, Relatorios.FolderId = -2 where Relatorios.ItemId = 34");
        }

        public override void Down()
        {
        }
    }
}