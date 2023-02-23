using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551200480)]
    public class FA1551200480_AdicionaColunasTabelaImpostoCsosn : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_item_icms").AddColumn("aliquotaIcms").AsDecimal(15, 4).WithDefaultValue(0);
            Alter.Table("nfce_item_icms").AddColumn("reducaoBcIcms").AsDecimal(15, 4).WithDefaultValue(0);
            Alter.Table("nfce_item_icms").AddColumn("valorBcIcms").AsDecimal(15, 2).WithDefaultValue(0);
            Alter.Table("nfce_item_icms").AddColumn("valorIcms").AsDecimal(15, 2).WithDefaultValue(0);

            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("aliquotaIcms");
            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("reducaoBcIcms");
            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("valorBcIcms");
            Delete.DefaultConstraint().OnTable("nfce_item_icms").InSchema("dbo").OnColumn("valorIcms");
        }

        public override void Down()
        {
        }
    }
}