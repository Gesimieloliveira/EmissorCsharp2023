using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616006552)]
    public class FA1616006552_AdicionarAutoCalcularTotaisItemNoItemNFe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_item").AddColumn("autoCalcularTotaisItem").AsBoolean().WithDefaultValue(true).NotNullable();
            Delete.DefaultConstraint().OnTable("nfe_item").InSchema("dbo").OnColumn("autoCalcularTotaisItem");
        }

        public override void Down()
        {
        }
    }
}