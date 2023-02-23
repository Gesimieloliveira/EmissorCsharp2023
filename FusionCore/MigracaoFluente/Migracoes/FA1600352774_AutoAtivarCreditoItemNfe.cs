using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1600352774)]
    public class FA1600352774_AutoAtivarCreditoItemNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("perfil_nfe")
                .AddColumn("autoAtivarCreditoItem").AsBoolean().NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("perfil_nfe")
                .InSchema("dbo")
                .OnColumn("autoAtivarCreditoItem");
        }

        public override void Down()
        {
        }
    }
}