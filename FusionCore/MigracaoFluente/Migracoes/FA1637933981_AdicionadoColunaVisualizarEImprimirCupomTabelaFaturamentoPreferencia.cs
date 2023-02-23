using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1637933981)]
    public class FA1637933981_AdicionadoColunaVisualizarEImprimirCupomTabelaFaturamentoPreferencia : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_preferencia").AddColumn("visualizarCupom").AsBoolean().NotNullable().SetExistingRowsTo(true);
            Alter.Table("faturamento_preferencia").AddColumn("imprimirCupom").AsBoolean().NotNullable().SetExistingRowsTo(true);
        }

        public override void Down()
        {
           
        }
    }
}