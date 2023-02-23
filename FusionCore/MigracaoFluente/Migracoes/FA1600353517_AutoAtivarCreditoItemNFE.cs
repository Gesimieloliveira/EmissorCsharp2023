using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1600353517)]
    public class FA1600353517_AutoAtivarCreditoItemNFE : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_item")
                .AddColumn("autoAtivarCreditoItem").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("nfe_item")
                .InSchema("dbo")
                .OnColumn("autoAtivarCreditoItem");
        }

        public override void Down()
        {
        }
    }
}