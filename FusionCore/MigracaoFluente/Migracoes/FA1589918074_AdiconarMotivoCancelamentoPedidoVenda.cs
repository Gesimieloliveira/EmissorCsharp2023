using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1589918074)]
    public class FA1589918074_AdiconarMotivoCancelamentoPedidoVenda : Migration
    {
        public override void Up()
        {
            Alter.Table("pedido_venda")
                .AddColumn("motivoCancelamento").AsAnsiString(255).NotNullable().SetExistingRowsTo(string.Empty);
        }

        public override void Down()
        {
        }
    }
}
