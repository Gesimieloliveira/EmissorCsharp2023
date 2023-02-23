using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1562959537)]
    public class FA1562959537_RefatoracaoPrecoMercadoriaNfe : Migration
    {
        public override void Up()
        {
            const string tbMercadoria = "nfe_item_mercadoria";

            Delete.Column("valorUnitarioComDesconto").FromTable(tbMercadoria);

            Rename.Column("valorUnitarioTotal").OnTable(tbMercadoria).To("totalBruto");
            Rename.Column("valorDescontoTotal").OnTable(tbMercadoria).To("totalDesconto");
            Rename.Column("valorUnitarioComDescontoTotal").OnTable(tbMercadoria).To("total");
        }

        public override void Down()
        {
        }
    }
}