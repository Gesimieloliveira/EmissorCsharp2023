using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1585058686)]
    public class FA1585058686_PedidoInternoConvertidoParaNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe").AddColumn("pedidoInternoSistema").AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable("nfe").InSchema("dbo").OnColumn("pedidoInternoSistema");
        }

        public override void Down()
        {
        }
    }
}