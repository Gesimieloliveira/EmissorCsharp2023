using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1628017813)]
    public class FN1628017813_InseridoColunaObservacaoNoItemFiscalNaTabelaProduto : Migration
    {
        public override void Up()
        {
            Alter.Table("produto")
                .AddColumn("usarObservacaoNoItemFiscal").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }

        public override void Down()
        {
            
        }
    }
}