using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1584554040)]
    public class FA1584554040_AlterarColunaNumeroPedidoParaVarchar : Migration
    {
        public override void Up()
        {
            Alter.Column("numeroPedido").OnTable("nfe_item").InSchema("dbo").AsAnsiString(15).NotNullable()
                .WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("nfe_item").InSchema("dbo").OnColumn("numeroPedido");
        }

        public override void Down()
        {
        }
    }
}