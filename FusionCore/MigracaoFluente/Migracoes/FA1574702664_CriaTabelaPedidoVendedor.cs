using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574702664)]
    public class FA1574702664_CriaTabelaPedidoVendedor : Migration
    {
        public override void Up()
        {
            Create.Table("pedido_vendedor")
                .WithColumn("pedidoVenda_id").AsInt32().NotNullable().PrimaryKey("pk_pedido_venda_pedido_vendedor")
                .WithColumn("vendedor_id").AsInt32().NotNullable()
                .ForeignKey("fk_pedido_vendedor__vendedor", "pessoa_vendedor", "pessoa_id");
        }

        public override void Down()
        {
        }
    }
}