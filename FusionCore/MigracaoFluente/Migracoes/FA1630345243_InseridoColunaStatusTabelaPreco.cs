using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1630345243)]
    public class FA1630345243_InseridoColunaStatusTabelaPreco : Migration
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