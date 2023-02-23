using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1571767690)]
    public class FA1571767690_MDFeCalculaCargaAutomatica : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe").AddColumn("isCalcularTotalCargaAutomatico").AsBoolean().WithDefaultValue(false);
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("isCalcularTotalCargaAutomatico");
        }

        public override void Down()
        {
        }
    }
}