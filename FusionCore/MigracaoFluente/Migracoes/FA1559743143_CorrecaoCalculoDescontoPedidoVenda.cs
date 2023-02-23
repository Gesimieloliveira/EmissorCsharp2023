using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1559743143)]
    public class FA1559743143_CorrecaoCalculoDescontoPedidoVenda : Migration
    {
        public override void Up()
        {
            Execute.Sql("update pedido_venda set totalDesconto = (totalProdutos - total);");
            Execute.Sql("update pedido_venda set percentualDesconto = case when totalProdutos > 0 then (totalDesconto * 100) / totalProdutos else 0 end;");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}