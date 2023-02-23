using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1591012821)]
    public class FA1591012821_AdicionarPrecosProdutoPedidoItem : Migration
    {
        public override void Up()
        {
            Alter.Table("pedido_produto")
                .AddColumn("precoCusto").AsDecimal(15, 4).NotNullable().WithDefaultValue(0.0M);

            Alter.Table("pedido_produto")
                .AddColumn("precoVenda").AsDecimal(15, 4).NotNullable().WithDefaultValue(0.0M);

            Delete.DefaultConstraint().OnTable("pedido_produto").OnColumn("precoCusto");
            Delete.DefaultConstraint().OnTable("pedido_produto").OnColumn("precoVenda");

            Execute.Sql(@"update pedido_produto set 
                pedido_produto.precoCusto = p.precoCusto
            , pedido_produto.precoVenda = p.precoVenda 
                from pedido_produto
                inner join produto as p 
                on p.id = pedido_produto.produto_id");


        }

        public override void Down()
        {
        }
    }
}