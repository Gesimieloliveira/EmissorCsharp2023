using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574426113)]
    public class FA1574426113_EstoqueMinimoEEstoqueMaximo : Migration
    {
        public override void Up()
        {
            Alter.Table("produto_estoque").AddColumn("estoqueMinimo").AsDecimal(16, 4).NotNullable()
                .WithDefaultValue(0.0m);

            Alter.Table("produto_estoque").AddColumn("estoqueMaximo").AsDecimal(16, 4).NotNullable()
                .WithDefaultValue(0.0m);

            Delete.DefaultConstraint().OnTable("produto_estoque").InSchema("dbo").OnColumn("estoqueMinimo");
            Delete.DefaultConstraint().OnTable("produto_estoque").InSchema("dbo").OnColumn("estoqueMaximo");
        }

        public override void Down()
        {
            
        }
    }
}