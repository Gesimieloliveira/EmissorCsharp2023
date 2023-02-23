using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1590414565)]
    public class FN1590414565_VendedorAtivo : Migration
    {
        public override void Up()
        {
            Alter.Table("vendedor").AddColumn("ativo").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("vendedor").InSchema("dbo").OnColumn("ativo");
        }

        public override void Down()
        {
        }
    }
}