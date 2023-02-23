using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1633003142)]
    public class FA1633003142_InseridoColunaImprimirFinalizacaoTabelaPreferenciaFaturamento : Migration
    {
        public override void Up()
        {
            Alter.Table("faturamento_preferencia").AddColumn("imprimirFinalizacao").AsBoolean().NotNullable().SetExistingRowsTo(true);
        }

        public override void Down()
        {
           
        }
    }
}