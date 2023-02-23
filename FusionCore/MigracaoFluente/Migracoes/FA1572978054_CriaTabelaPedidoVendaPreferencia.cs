using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1572978054)]
    public class FA1572978054_CriaTabelaPedidoVendaPreferencia : Migration
    {
        public override void Up()
        {
            Create.Table("pedido_venda_preferencia")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_pedido_venda_preferencia")
                .WithColumn("identificadorMaquina").AsAnsiString(40).NotNullable()
                .WithColumn("impressora").AsAnsiString(255).NotNullable()
                .WithColumn("imprimeAposFinalizar").AsBoolean().NotNullable()
                .WithColumn("visualizarAposFinalizar").AsBoolean().NotNullable()
                .WithColumn("imprimeDuasVias").AsBoolean().NotNullable();
        }

        public override void Down()
        {
        }
    }
}