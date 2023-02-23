using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1610546254)]
    public class FA1610546254_AdicionarTabelaPrecoPedidoVenda : Migration
    {
        public override void Up()
        {
            Alter.Table("pedido_venda")
                .AddColumn("tabelaPreco_id")
                .AsInt32().ForeignKey("fk_pedido_venda__tabela_preco", "tabela_preco", "id")
                .Nullable();
        }

        public override void Down()
        {
        }
    }
}