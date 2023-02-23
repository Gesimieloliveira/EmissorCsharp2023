using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1630345494)]
    public class FN1630345494_InseridoColunaStatusTabelaPreco : Migration
    {
        public override void Up()
        {
            Alter.Table("tabela_preco").AddColumn("status").AsBoolean().NotNullable().SetExistingRowsTo(true);
        }

        public override void Down()
        {
            
        }
    }
}